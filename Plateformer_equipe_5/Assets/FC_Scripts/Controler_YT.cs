using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_YT : MonoBehaviour
{    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xAxis;  
    [System.NonSerialized]
    public float verticalSpeed;
    [System.NonSerialized]
    public float horizontalSpeed;
    private Vector3 playerMove;
    RaycastHit2D groundChecker;
    public LateralCollision_FC wallCollision;

    [System.NonSerialized]
    public bool jumpKey, jumpKeyDown, jumpKeyUp, runKey, runKeyDown, runKeyUp,
        crouchKey, crouchKeyDown, crouchKeyUp;

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
    [SerializeField]
    public BoxCollider2D boxCollider2d;
    [SerializeField]
    public float gravity;    
    public float decalage;

    //Move parameters
    float walkingVelocity;
    float runningVelocity;
    float velocityMultiplicator = 1;
    bool isRunning;
    bool moveFreeze;    

    //Run parameters
    [SerializeField]
    [Header("Move Parameters")]
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    private float curSpeed;
    float accelerationTime;
    [SerializeField]
    public float accelerationTimeEnd;
    [SerializeField]
    float walkSpeed;

    //Variables Jump
    [Header("Jump")]    
    [SerializeField]
    float jumpFixMaxHeight;
    [SerializeField]
    float jumpStrength;
    [SerializeField]
    float glideTime;

    bool highestPointReached;
    bool isJumping;
    bool jumpInputMaintain;
    bool jumpGravityAllowed;
    float jumpHeight;
    float jumpMaxHeight;

    void Start()
    {
        gravity = 0.5f;
        acceleration = 0.5f;
        deceleration = -0.01f;
        glideTime = 0.1f;
        jumpStrength = 10;
        slidingDecelaration = -0.002f;
        crouchSpeed = 1.5f;
        jumpFixMaxHeight = 0.8f;
        curSpeed = 0;
        maxSpeed = 1.5f;
        walkSpeed = 2f;

        decalage = 0.06f;
    }

    void Awake()
    {
        crouchSize = new Vector2(0.16f, 0.08f);
        idleSize = new Vector2(0.16f, 0.16f);
        slidingVelocity = 0;
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        groundChecker = GroundDetector();

        StandStill();
        Walk();

        //Run Test
        if (Input.GetAxis("Horizontal") != 0 && runKey) isRunning = true;
        else
        {
            isRunning = false;
            accelerationTime = 0f;
        }

        Run();
        SpriteFlip();

        //Enter Jump state
        if (jumpKeyDown && isJumping == false && IsGrounded())
        {
            EnterJump();
            jumpMaxHeight = transform.localPosition.y + jumpFixMaxHeight;
        }

        if (isJumping == true)
        {
            jumpHeight = transform.localPosition.y;           
        }

        //Jump reaches it limit
        if (jumpInputMaintain == true && (jumpHeight == jumpMaxHeight || jumpHeight > jumpMaxHeight) && isJumping == true)
        {
            highestPointReached = true;
        }

        if (!isJumping)
        {
            Crouch();
            Slide();
        }

        CharacterSizeCheck();

        //Horizontal Speed Calculation
        horizontalSpeed = (walkingVelocity + runningVelocity + slidingVelocity) * velocityMultiplicator * wallCollision.wallContact;

        playerMove = new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
        transform.Translate(playerMove);
    }

    //Crouch
    private void Crouch()
    {
        if (crouchKeyDown && IsGrounded() && !isRunning) //Freeze de début d'accroupissement 
        {
            MoveFreeze(0.2f);
            waitBeforeStand = true;
            isCrouching = true;
            StartCoroutine("Crouch2StandCooldown");
        }
        else if (crouchKeyDown && IsGrounded() && isRunning) //Lancement du Slide
        {
            waitBeforeStand = true;
            isSliding = true;
        }
        else if ((!crouchKey || crouchKeyUp) && IsGrounded() && !isRunning && isCrouching) //Relâchement bouton pendant accroupissement
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
    IEnumerator Crouch2StandCooldown()//Modifie le bool moveFreeze en true tant que x temps n'est pas passé
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

    private void Slide()
    {
        if (isSliding && velocityMultiplicator != 0 && crouchKey) //Sliding with Run Speed and start decelerating
        {
            slidingVelocity = (maxSpeed + walkSpeed) * xAxis;
            StartCoroutine("SlideDeceleration");
        }
        else if (isSliding && (velocityMultiplicator == 0 || crouchKeyUp)) //Slide has totally decelerated
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
        if (!isSliding && !isCrouching)
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, new Vector2( boxCollider2d.bounds.size.x - decalage,
            boxCollider2d.bounds.size.y), 0f, Vector2.down, 0.05f, platformLayerMask);

        Color rayColor;
        rayColor = Color.green;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x - decalage, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor); //gauche
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x - decalage, 0), Vector2.down * (boxCollider2d.bounds.extents.y), rayColor); //droite
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x - decalage, boxCollider2d.bounds.extents.y),
            Vector2.right * (boxCollider2d.bounds.extents.x), rayColor); //bas

        return raycastHit;
    }
    public bool IsGrounded()
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
            if (isJumping)
            {
                if (jumpGravityAllowed) verticalSpeed -= gravity;
                else { }                
            }
            else verticalSpeed -= gravity;          
        }        
    }

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

    //Jump
    void EnterJump()
    {
        isJumping = true;
        jumpInputMaintain = true;

        StartCoroutine("JumpInputCheck");
        StartCoroutine("JumpMovement");
    }
    //Check if the Jump Input is maintained by the player
    IEnumerator JumpInputCheck()
    {
        yield return new WaitForSeconds(0.01f);
        if (jumpInputMaintain == true && jumpKey)
        {
            StartCoroutine("JumpInputCheck");
        }

        if ((jumpInputMaintain == true && !jumpKey) || (jumpInputMaintain == true && jumpHeight > jumpMaxHeight))
        {
            jumpInputMaintain = false;
        }
    }
    //Jump movement until highest point
    IEnumerator JumpMovement()
    {
        Vector2 playerInput;
        playerInput.y = Input.GetAxis("Jump");
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        yield return new WaitForSeconds(0.01f);
        if (isJumping == true && jumpInputMaintain == true)
        {
            Vector3 arise =
                new Vector3(0, playerInput.y * jumpStrength * Time.deltaTime, 0);
            transform.localPosition += arise;

            StartCoroutine("JumpMovement");
        }
        else if (isJumping == true && (jumpInputMaintain == false || highestPointReached == true))
        {
            yield return new WaitForSeconds(glideTime);
            StartCoroutine("JumpFall");
        }
    }
    IEnumerator JumpFall()
    {
        yield return new WaitForSeconds(0.01f);
        if (!IsGrounded() && isJumping == true)
        {
            jumpGravityAllowed = true;
            StartCoroutine("JumpFall");
        }
        else if (IsGrounded() && isJumping == true)
        {
            isJumping = false;
            jumpGravityAllowed = false;
        }
    }
}