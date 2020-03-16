using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_YT : MonoBehaviour
{    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xAxis; 
    public float gravity;
    private float verticalSpeed;
    private float horizontalSpeed;
    private Vector3 playerMove;
    private bool onGround;

    //GoundCheck Composents
    public LayerMask platformLayerMask;
    public BoxCollider2D boxCollider2d;

    //Move parameters
    [SerializeField]
    float walkSpeed;
    float runSpeed;
    float walkingVelocity;
    float runningVelocity;
    bool isWalking;
    bool isRunning;

    //Run parameters
    public float acceleration = 1.0f;
    public float deceleration = - 1f;
    public float maxSpeed = 2.0f;

    private float curSpeed = 0.0f;


    void Awake()
    {
        
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        StandStill();
        Walk();
        Run();

        if ( horizontalSpeed == walkSpeed)
        {
            isWalking = true;
        }
        else if (horizontalSpeed != walkSpeed)
        {
            isWalking = false;
        }

        //Horizontal Speed Calculation
        horizontalSpeed = walkingVelocity + runningVelocity;

        playerMove = new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
        transform.Translate(playerMove);
    }

    private float Walk()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        walkingVelocity = playerInput.x * walkSpeed;
        return walkingVelocity;
    }

    private float Run()
    {
        if (isWalking == true && Input.GetButtonDown("Fire3"))
        {
            float curSpeed = 1;
            runningVelocity = curSpeed;
            if (curSpeed < maxSpeed)
            {
                StartCoroutine("Acceleration");
            }                       
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            StartCoroutine("Deceleration");
        }
        return runningVelocity;
    }

    IEnumerator Acceleration()
    {
        yield return new WaitForSeconds(0.01f);
        curSpeed += acceleration;

        if (curSpeed > maxSpeed)
        {
            curSpeed = maxSpeed;
        }
        else
        {
            StartCoroutine("Acceleration");
        }
    }

    IEnumerator Deceleration()
    {
        yield return new WaitForFixedUpdate();
        curSpeed += deceleration;

        if (curSpeed == 0 || curSpeed < 0)
        {
            curSpeed = 0;
        }
        else
        {
            StartCoroutine("Deceleration");
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0, platformLayerMask);
        Color rayColor;
        rayColor = Color.green;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);
        
        return raycastHit.collider != null;
    }

    private void StandStill()
    {
        if (isGrounded())
        {
            verticalSpeed = 0;
        }
        else if (!isGrounded())
        {
            verticalSpeed = -gravity;
        }
    }
}