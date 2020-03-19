using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_YT : MonoBehaviour
{
    private Rigidbody2D rb;

    private float xAxis;

    public float moveSpeed;

    public float jumpForce;

    public float groundDectectionRadius;

    [SerializeField]
    Animator animator;

    private bool bRight;

    public LayerMask whatIsGround;

    private Transform groundDetectionObject;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetectionObject = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        doJump();
        Direction();

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, 0f);


    }

    private void Direction()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bRight = true;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bRight = false;
        }

        spriteRenderer.flipX = bRight;
    }

    void doJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded())
        {
            rb.velocity += Vector2.up * jumpForce;
        }

        bool isGrounded()
        {
            return Physics2D.OverlapCircle(groundDetectionObject.position, groundDectectionRadius, whatIsGround);
        }
    }
}