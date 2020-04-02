using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_FC : MonoBehaviour
{    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xAxis;     
    private float verticalSpeed;
    private float horizontalSpeed;
    private Vector3 playerMove;
    RaycastHit2D groundChecker;


    //Cooldown (WaitForATime) Composents
    float timeToWait;

    //Idle Composents
    public Vector2 idleSize;
    public Sprite idleSprite;

    //Crouch parameters
    public Vector2 crouchSize;
    public Sprite crouchSprite;
    bool isCrouching;
    bool waitBeforeStand;
    [SerializeField]
    float crouchSpeed;

    //Slide parameters
    bool isSliding;
    [SerializeField]
    float slidingDecelaration;
    float slidingVelocity;

    //GoundCheck Composents
    [SerializeField]
    [Header ("GroundCheck Composents")]
    
    public LayerMask platformLayerMask;
    public BoxCollider2D boxCollider2d;
    public float gravity;    

    //Move parameters    
    float walkingVelocity;
    float runningVelocity;
    float velocityMultiplicator = 1;
    bool isWalking;
    bool isRunning;
    bool moveFreeze;

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
        crouchSize = new Vector2(0.16f, 0.08f);
        idleSize = new Vector2(0.16f, 0.16f);
        slidingVelocity = 0;
    }

    private void FixedUpdate()
    {     
      
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        groundChecker = GroundDetector();

        StandStill();
        Walk();
        Run();
        Crouch();
        Slide();
        CharacterSizeCheck();
        SpriteFlip();

        if (horizontalSpeed == walkSpeed || horizontalSpeed == - walkSpeed)
        {
            isWalking = true;
        }
        else
            isWalking = false;

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
        horizontalSpeed = (walkingVelocity + runningVelocity + slidingVelocity) * velocityMultiplicator;

        playerMove = new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
        transform.Translate(playerMove);
    }

    //Crouch
    private void Crouch()
    {
        if (Input.GetButtonDown("Crouch") && IsGrounded() && !isRunning) //Freeze de début d'accroupissement 
        {
            MoveFreeze(0.2f);
            waitBeforeStand = true;
            isCrouching = true;
            StartCoroutine("Crouch2StandCooldown");
        }
        else if (Input.GetButtonDown("Crouch") && IsGrounded() && isRunning) //Lancement du Slide
        {
            waitBeforeStand = true;
            isSliding = true;
        }
        else if ((!Input.GetButton("Crouch") || Input.GetButtonUp("Crouch")) && IsGrounded() && !isRunning && isCrouching) //Relâchement bouton pendant accroupissement
        {
            if (!waitBeforeStand) //Si lâcher bouton après fin waitBeforeStand
            {
                MoveFreeze(0.1f);
                isCrouching = false;
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);                
            }
            else
            {

            }
        }
    }

    private void Slide()
    {
        if (isSliding && velocityMultiplicator != 0 && Input.GetButton("Crouch")) //Sliding with Run Speed and start decelerating
        {
            slidingVelocity = (maxSpeed + walkSpeed) * xAxis;
            StartCoroutine("SlideDeceleration");
        }
        else if (isSliding && (velocityMultiplicator == 0 || Input.GetButtonUp("Crouch"))) //Slide has totally decelerated
        {
            StopCoroutine("SlideDeceleration");
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
            slidingVelocity = 0f;
            velocityMultiplicator = 1f;
            isSliding = false;
        }
        else
        {
            
        }
    }
    IEnumerator SlideDeceleration()
    {
        yield return new WaitForSeconds(0.01f);
        if ((velocityMultiplicator == 0 || velocityMultiplicator < 0))
        {
            velocityMultiplicator = 0;
        }
        else
        {
            velocityMultiplicator += slidingDecelaration;
            StartCoroutine("SlideDeceleration");
        }
    }

    IEnumerator Crouch2StandCooldown()// Modifie le bool moveFreeze en true tant que x temps n'est pas passé
    {
        yield return new WaitForSeconds(0.4f);
        waitBeforeStand = false;
    }    
    private void MoveFreeze(float x)
    {
        timeToWait = x;
        moveFreeze = true;
        StartCoroutine("WaitForATime");
    } 
    IEnumerator WaitForATime()
    {
        yield return new WaitForSeconds(timeToWait);
        moveFreeze = false;
    }

    //Size check
    void CharacterSizeCheck()
    {
        if (!isCrouching && !isSliding)
        {
            boxCollider2d.size = idleSize;
            spriteRenderer.sprite = idleSprite;
        }
        else if (isSliding || isCrouching)
        {
            boxCollider2d.size = crouchSize;
            spriteRenderer.sprite = crouchSprite;
        }
    }

    //Walk
    private float Walk()
    {
        if ((!isCrouching || !isSliding) && !moveFreeze)
        {
            walkingVelocity = xAxis * walkSpeed;
        }
        else if ((isCrouching || isSliding) && !moveFreeze)
        {
            walkingVelocity = xAxis * crouchSpeed;
        }
        else
        {
            walkingVelocity = 0;
        }
        return walkingVelocity;
    }


    //Run
    private float Run()
    {
        runningVelocity = xAxis * curSpeed;
        if (!isSliding)
        {
            if (isRunning == true)
            {            
                if (curSpeed < maxSpeed)
                {
                isRunning = true;
                StartCoroutine("Acceleration");
                }       
                else if (curSpeed >= maxSpeed)
                    curSpeed = maxSpeed;
            }
            else if (isRunning == false) StartCoroutine("Deceleration");           
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


    //Ground and stand
    private RaycastHit2D GroundDetector()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.05f, platformLayerMask);
        Color rayColor;
        rayColor = Color.green;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);

        return raycastHit;
    }
    private bool IsGrounded()
    {        
        return groundChecker.collider != null;
    }   
    private void StandStill()
    {
        if (IsGrounded())
        {
            verticalSpeed = 0;
        }
        else if (!IsGrounded())
        {
            verticalSpeed -= gravity;
        }        
    }
    
    /*private void StandStill()
    {
        if (IsGrounded() && (boxCollider2d.bounds.extents.y - groundChecker.distance) <= boxCollider2d.bounds.extents.y )
        {
            transform.localPosition = new Vector2(transform.localPosition.x,transform.localPosition.y + (boxCollider2d.bounds.extents.y - groundChecker.distance)) ;
        }
        else if (IsGrounded() && (boxCollider2d.bounds.extents.y - groundChecker.distance) >= boxCollider2d.bounds.extents.y)
        {

        }
        else if (!IsGrounded())
        {
            verticalSpeed -= gravity;
        }        
    }
    */    

    void SpriteFlip()
    {
        if (Input.GetAxis("Horizontal") > 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetAxis("Horizontal") < 0 && spriteRenderer.flipX == true)
        {
             spriteRenderer.flipX = false;
        }
        else { }
    }    
}