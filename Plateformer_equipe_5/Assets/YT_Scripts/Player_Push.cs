using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Push : MonoBehaviour
{
    public LayerMask boxLayermask;
    RaycastHit2D boxDetector;
    private BoxCollider2D boxCollider;
    bool right;
    public float rayLength;
    bool isPushing;
    bool movingObject;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        rayLength = 0.3f;
    }

    void Update()
    {
        Movement();
        Pushingconditions();
    }


    bool Movement()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            right = true;
        }

        else if (Input.GetAxis("Horizontal") < 0)
        {
            right = false;
        }

        return right;
    }

    public RaycastHit2D BoxDetector()
    {

        if (right == true)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y), Vector2.right,
                rayLength, boxLayermask); 
            Debug.DrawRay(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y), Vector2.right*rayLength, Color.magenta);
            boxDetector = raycastHit;
        }

        else if (right == false)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y),
                Vector2.left, rayLength, boxLayermask); 
            Debug.DrawRay(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y), Vector2.left*rayLength, Color.magenta);
            boxDetector = raycastHit;
        }

        return boxDetector;
    }

    bool Pushingconditions()
    {
        if (BoxDetector() && Input.GetKey(KeyCode.E))
        {
            isPushing = true;
        }
        else
        {
            isPushing = false;
        }
        return isPushing;
    }

    bool PushingConditionValidated()
    {
        if (isPushing == true)
        {



        }

        return movingObject;
    }


}
