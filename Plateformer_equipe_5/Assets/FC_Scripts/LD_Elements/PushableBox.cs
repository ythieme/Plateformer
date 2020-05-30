using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    FearScript_FC fear;

    RaycastHit2D hit;
    RaycastHit2D wallLeft;
    RaycastHit2D wallRight;
    public LayerMask floor;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Vector2 startPosition;

    public float width;
    public float width2;
    public float lengthBonus;
    bool isFalling;
    public bool isBesideWall;

    void Start()
    {
        isFalling = true;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        fear = GameObject.Find("FearObject").GetComponent<FearScript_FC>();
        startPosition = transform.position;
    }    
    void Update()
    {
        if (fear.fear == 0 || fear.fear < 0) transform.Translate(new Vector3(startPosition.x - transform.position.x, startPosition.y - transform.position.y));

        hit = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)), Vector2.right, width * 2, floor);
        wallLeft = Physics2D.Raycast(new Vector2(transform.position.x - boxCollider.bounds.extents.x - 0.02f, transform.position.y - width2), Vector2.up, width2 * 2, floor);
        wallRight = Physics2D.Raycast(new Vector2(transform.position.x + boxCollider.bounds.extents.x + 0.02f, transform.position.y - width2), Vector2.up, width2 * 2, floor);
        
        Debug.DrawLine(new Vector2(transform.position.x - width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)),
            new Vector2(transform.position.x + width, transform.position.y - (boxCollider.bounds.extents.y + lengthBonus)), Color.red);

        Debug.DrawLine(new Vector2(transform.position.x - boxCollider.bounds.extents.x - 0.02f, transform.position.y - width2),
            new Vector2(transform.position.x - boxCollider.bounds.extents.x - 0.02f, transform.position.y + width2), Color.blue);//Left

        Debug.DrawLine(new Vector2(transform.position.x + boxCollider.bounds.extents.x + 0.02f, transform.position.y - width2),
            new Vector2(transform.position.x + boxCollider.bounds.extents.x + 0.02f, transform.position.y + width2), Color.blue);//Right

        if (wallLeft || wallRight) 
        { 
            isBesideWall = true;
        }            
        else isBesideWall = false;

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
                GetComponent<AudioSource>().Play();
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
