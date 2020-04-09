using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Push : MonoBehaviour
{
    public Player_Push pushingTrigered;
    private int movement;
    private BoxCollider2D boxCollider2d;
    public LayerMask boxType;
    public float gravity;
    public float raylenght;
    public LayerMask groundLayerMask;
    public float overlapCircleRadius;
    private int isGrounded;
    //RaycastHit2D groundDectector;

   

    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        raylenght = 5f;
    }
    void Update()
    {
        //GrounDetector();
    }

    int BoxMovement()
    {
        if (pushingTrigered == true)
        {



        }
        return movement;
    }

   /* public Physics2D GrounDetector()
    {
        {
            Physics2D.OverlapCircle(new Vector2(boxCollider2d.bounds.center.x, 5f), ContactFilter2D.SetLayerMask, boxCollider2d[]) ;
        }

        return ;
    }

    /*private RaycastHit2D GroundDetector()
    {
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(boxCollider2d.bounds.max.x, boxCollider2d.bounds.min.y), Vector2.right,
                raylenght, boxType);
         Debug.DrawRay(new Vector2(boxCollider2d.bounds.min.x, boxCollider2d.bounds.min.y), Vector2.right * raylenght, Color.yellow);
         groundDectector = raycastHit;
        }

        return groundDectector;
    } */
}
