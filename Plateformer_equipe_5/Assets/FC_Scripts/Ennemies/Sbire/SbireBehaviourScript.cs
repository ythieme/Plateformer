using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SbireBehaviourScript : MonoBehaviour
{
    public Transform wayPointTransform;
    public Transform startPositionTransform;

    public SpriteRenderer sp;

    public Vector3 wayPoint;
    public Vector3 startPosition;
    
    [Range(0.0001f,0.005f)] public float speed;
    [Range(0.1f, 2f)] public float stopDuration;

    bool stop;
    bool goingWayPoint;
    bool goingStartPosition;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        goingStartPosition = true;
        wayPoint = wayPointTransform.position;
        startPosition = startPositionTransform.position;
    }
    private void Update()
    {
        SbireMovement();
    }    

    void SbireMovement()
    {
        if (transform.position == startPosition && !stop && goingStartPosition) //Reach StartPosition
        {
            StartCoroutine(MovementStop(stopDuration));
            goingStartPosition = false;
            goingWayPoint = true;
        }
        else if (transform.position == startPosition && !stop && goingWayPoint) //Go WayPoint
        {
            StartCoroutine(MoveTowardPlace(wayPoint, speed));
            if (wayPoint.x - startPosition.x > 0) sp.flipX = true;
        }
        else if (transform.position == wayPoint && !stop && goingWayPoint) //Reach WayPoint
        {
            StartCoroutine(MovementStop(stopDuration));
            goingWayPoint = false;
            goingStartPosition = true;            
        }
        else if (transform.position == wayPoint && !stop && goingStartPosition) //Go StartPosition
        {
            StartCoroutine(MoveTowardPlace(startPosition,speed));
            if (startPosition.x - wayPoint.x < 0) sp.flipX = false;
        }
    }

    IEnumerator MoveTowardPlace(Vector2 direction,float speed)
    {
        yield return new WaitForSeconds(0.01f);
        if (!stop)
        {
            transform.position = Vector2.MoveTowards(transform.position, direction, speed);
            StartCoroutine(MoveTowardPlace(direction, speed));
        }
    }

    IEnumerator MovementStop(float duration)
    {
        stop = true;
        yield return new WaitForSeconds(duration);
        stop = false;
    }
}
