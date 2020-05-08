using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrPontBodyMovement : MonoBehaviour
{
    public Transform playerPosition;

    Vector2 heightDifference;
    Vector2 direction;

    float minHeight;
    float maxHeight;
    public float maxGap;
    public float minGap;
    public float speed;
    float playerHeight;

    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        minHeight = transform.position.y - minGap;
        maxHeight = transform.position.y + maxGap;
    }
    void Update()
    {
        playerHeight = playerPosition.position.y;
        heightDifference = new Vector2(0, playerHeight - transform.position.y);
        direction = new Vector2(0, transform.localPosition.y + heightDifference.y);
        UpAndDown(direction);        
    }
    void UpAndDown(Vector2 direction)
    {
        if (!((transform.position.y < minHeight || transform.position.y == minHeight) && heightDifference.y < 0) &&
            !((transform.position.y > maxHeight || transform.position.y == maxHeight) && heightDifference.y > 0))
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, direction, speed);
        }        
    }    
}