using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    RaycastHit2D hit;
    RaycastHit2D fallingHit1;
    RaycastHit2D fallingHit2;
    RaycastHit2D fallingHit3;
    RaycastHit2D fallingHit4;    
    public LayerMask floor;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    public float width;
    public float lengthBonus;
    bool isFalling;   

    void Start()
    {
        isFalling = true;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }    
    void Update()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)), Vector2.right, width * 2, floor);

        Debug.DrawLine(new Vector2(transform.position.x - width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)),
            new Vector2(transform.position.x + width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)), Color.red);
               

        if (!hit && !isFalling)
        {
            isFalling = true;
            rb.isKinematic = false;
            rb.freezeRotation = false;
        }
        else if (isFalling)
        {
            if (rb.velocity.y >= 0f)
            {
                rb.freezeRotation = true;
                rb.velocity = new Vector2(0, 0);
                transform.eulerAngles = new Vector3(0,0,0);
                rb.isKinematic = true;
                isFalling = false;
            }
            else { }
        }
        else
        {            
            rb.isKinematic = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.velocity = new Vector2(0, 0);
        }
    }
    
}
