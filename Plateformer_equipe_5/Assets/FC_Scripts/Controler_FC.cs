using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_YT : MonoBehaviour
{    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xAxis;     
    private float verticalSpeed;
    private float horizontalSpeed;
    private Vector3 playerMove;
    private bool onGround;

    //GoundCheck Composents
    [SerializeField]
    [Header ("GroundCheck Composents")]
    
    public LayerMask platformLayerMask;
    public BoxCollider2D boxCollider2d;
    public float gravity;

    //Move parameters    
    float walkingVelocity;
    float runningVelocity;
    bool isWalking;
    bool isRunning;

    //Run parameters
    [SerializeField]
    [Header("Move Parameters")]
    float walkSpeed;
    public float acceleration = 1f;
    public float deceleration = - 1f;
    public float maxSpeed = 2f;
    private float curSpeed = 0f;
    float accelerationTime;
    public float accelerationTimeEnd = 1f;


    void Awake()
    {

    }

    private void FixedUpdate()
    {     
      
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        StandStill();
        Walk();
        Run();
        Crouch();

        if (horizontalSpeed == walkSpeed || horizontalSpeed == - walkSpeed)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (horizontalSpeed != 0 && Input.GetButton("Fire3"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
            accelerationTime = 0f;
        }

        //Horizontal Speed Calculation
        horizontalSpeed = walkingVelocity + runningVelocity;

        playerMove = new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
        transform.Translate(playerMove);
    }



    private void Crouch()
    {
        if (Input.GetKey("Fire1") && IsGrounded() && !isRunning)
        {
            boxCollider2d.size = new Vector2 (0.16f, 0.08f);
        }
    }

    private float Walk()
    {
        walkingVelocity = xAxis * walkSpeed;
        return walkingVelocity;
    }

    private float Run()
    {
        runningVelocity = xAxis * curSpeed;
        if (isRunning == true)
        {            
            if (curSpeed < maxSpeed)
            {
                isRunning = true;
                StartCoroutine("Acceleration");
            }       
            else if (curSpeed >= maxSpeed)
            {
                curSpeed = maxSpeed;
            }
        }
        else if (isRunning == false)
        {
            StartCoroutine("Deceleration");
        }
        return runningVelocity;
    }

    IEnumerator Acceleration()
    {
        yield return new WaitForSeconds(0.01f);

        if (isRunning)
        {
            if (accelerationTime >= accelerationTimeEnd)
            {
                curSpeed = maxSpeed;
            }
            else if (accelerationTime < accelerationTimeEnd)
            {
                curSpeed += acceleration;
                accelerationTime += Time.deltaTime;
                StartCoroutine("Acceleration");
            }
        }
        else
        {
            StartCoroutine("Deceleration");
        }
    }

    IEnumerator Deceleration()
    {
        yield return new WaitForSeconds(0.01f);
        if (curSpeed == 0 || curSpeed < 0)
        {
            curSpeed = 0;
            isRunning = false;
        }
        else
        {
            curSpeed += deceleration;
            StartCoroutine("Deceleration");
        }
    }

    private bool IsGrounded()
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
        if (IsGrounded())
        {
            verticalSpeed = 0;
        }
        else if (!IsGrounded())
        {
            verticalSpeed = -gravity;
        }
    }
}