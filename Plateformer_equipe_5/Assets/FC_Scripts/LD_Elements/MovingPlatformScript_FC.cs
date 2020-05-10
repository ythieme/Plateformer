using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript_FC : MonoBehaviour
{
    public Controler_YT controler;
    
    public Transform wayPointTransform;
    public Transform startPositionTransform;
    public LayerMask playerLayerMask;
    public BoxCollider2D boxcollider;

    public Vector3 wayPoint;
    public Vector3 startPosition;

    [Range(0.5f, 5f)] public float speed;
    [Range(0.1f, 2f)] public float stopDuration;
    float step;

    bool stop;
    bool goingWayPoint;
    bool goingStartPosition;
    bool playerDetected;

    float decalageY = 0.05f;

    float originX;
    float originY;

    float firstXPosition;
    float secondXPosition;
    float firstYPosition;
    float secondYPosition;
    [System.NonSerialized] public float actualSpeedX;
    [System.NonSerialized] public float actualSpeedY;

    void Start()
    {
        controler = GameObject.FindGameObjectWithTag("Player").GetComponent<Controler_YT>();
        playerDetected = false;
        goingStartPosition = true;
        wayPoint = wayPointTransform.position;
        startPosition = startPositionTransform.position;
        StartCoroutine(ActualSpeed());
    }

    void FixedUpdate()
    {
        originX = (boxcollider.bounds.center.x - boxcollider.bounds.extents.x);
        originY = (boxcollider.bounds.center.y + boxcollider.bounds.extents.y + decalageY);
        step = speed * Time.deltaTime;
        Movement();
        PlayerDetector();
        StickThePlayer();
    }
    void Movement()
    {
        if (transform.position == startPosition && !stop && goingStartPosition) //Reach StartPosition
        {
            StartCoroutine(MovementStop(stopDuration));
            goingStartPosition = false;
            goingWayPoint = true;
        }
        else if (transform.position == startPosition && !stop && goingWayPoint) //Go WayPoint
        {
            StartCoroutine(MoveTowardPlace(wayPoint, step));
        }
        else if (transform.position == wayPoint && !stop && goingWayPoint) //Reach WayPoint
        {
            StartCoroutine(MovementStop(stopDuration));
            goingWayPoint = false;
            goingStartPosition = true;
        }
        else if (transform.position == wayPoint && !stop && goingStartPosition) //Go StartPosition
        {
            StartCoroutine(MoveTowardPlace(startPosition, step));
        }
    }
    IEnumerator MoveTowardPlace(Vector2 direction, float speed)
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

    void StickThePlayer()
    {
        if (PlayerDetector() && !playerDetected)
        {
            playerDetected = true;
            controler.movingPlatformXVelocity = actualSpeedX;
            controler.movingPlatformYVelocity = actualSpeedY;
        }
        else if (PlayerDetector() && playerDetected)
        {
            controler.movingPlatformXVelocity = actualSpeedX;
            controler.movingPlatformYVelocity = actualSpeedY;
        }
        else if (!PlayerDetector() && playerDetected)
        {
            playerDetected = false;
            controler.movingPlatformXVelocity = 0f;
            controler.movingPlatformYVelocity = 0f;
        }
    }

    IEnumerator ActualSpeed()
    {
        firstXPosition = transform.position.x;
        firstYPosition = transform.position.y;

        yield return new WaitForSeconds(0.01f);

        secondXPosition = transform.position.x;
        secondYPosition = transform.position.y;

        actualSpeedX = (secondXPosition - firstXPosition);
        actualSpeedY = (secondYPosition - firstYPosition);
        StartCoroutine(ActualSpeed());
    }

    public RaycastHit2D PlayerDetector()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2 (originX, originY),
            Vector2.right, boxcollider.bounds.size.x,  playerLayerMask);

        Debug.DrawRay(new Vector2(originX, originY),
            transform.TransformDirection(new Vector2(boxcollider.bounds.size.x, 0)), Color.blue);

        return raycastHit;
    }
}
