using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler_YT : MonoBehaviour
{    
    [SerializeField] SpriteRenderer spriteRenderer;

    private float xAxis;  
    [System.NonSerialized] public float verticalSpeed;
    [System.NonSerialized] public float horizontalSpeed;
    private Vector3 playerMove;
    RaycastHit2D groundChecker;

    //External Scripts
    public BoxPush_FC push;
    public LateralCollision_FC wallCollision;
    public HeadCollision_FC headCollision;
    public GripScript_FC grip;
    public DontFallAnymore_TheScript dontFall;

    [System.NonSerialized] public bool jumpKey, jumpKeyDown, jumpKeyUp, runKey, runKeyDown, runKeyUp, run2Key, run2KeyDown, run2KeyUp,
        crouchKey, crouchKeyDown, crouchKeyUp;

    //Cooldown (WaitForATime) Composents
    float timeToWait;

    //Idle Composents
    public Vector2 idleSize;
    public Sprite idleSprite;

    //Crouch parameters
    public Vector2 crouchSize;
    public Sprite crouchSprite;    
    bool waitBeforeStand;
    [SerializeField] float crouchSpeed;
    [System.NonSerialized] public bool isCrouching;    

    //GoundCheck Composents
    [Header ("GroundCheck Composents")]
    [SerializeField] public LayerMask platformLayerMask;
    [SerializeField] public BoxCollider2D boxCollider2d;
    [SerializeField] public float gravity;
    [SerializeField] public float decalage;

    //Move parameters
    float walkingVelocity;
    float runningVelocity;
    [System.NonSerialized] public float velocityMultiplicator = 1;
    [System.NonSerialized] public float movingPlatformXVelocity;
    [System.NonSerialized] public float movingPlatformYVelocity;
    [System.NonSerialized] public bool isRunning;
    [System.NonSerialized] public bool moveFreeze;
    [System.NonSerialized] public bool isPushing;
            
    [Header("Move Parameters")] //Run parameters
    [SerializeField]
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    private float curSpeed;
    float accelerationTime;
    [SerializeField] public float accelerationTimeEnd;
    [SerializeField] float walkSpeed;
                
    [Header("Jump")] //Variables Jump
    [SerializeField] float jumpFixMaxHeight;
    [SerializeField] float jumpStrength;
    [SerializeField] float glideTime;
    [System.NonSerialized] public bool highestPointReached, isJumping, jumpGravityAllowed;
        
    [Header("Slide Parameters")] //Slide parameters    
    [SerializeField] float slidingDecelaration;
    [SerializeField] float slidingVelocity;
    [SerializeField] bool slideCooldown;
    [SerializeField] float slideCooldownDuration;
    [System.NonSerialized] public bool isSliding;

    [Header("Animation")]
    public Animator anim;

    bool jumpInputMaintain;

    float jumpHeight;
    float jumpMaxHeight;

    void Start()
    {
        dontFall = GetComponent<DontFallAnymore_TheScript>();
        anim = GetComponent<Animator>();

        movingPlatformXVelocity = 0f;
        gravity = 20f;
        acceleration = 0.5f;
        deceleration = -0.01f;
        glideTime = 0.14f;
        jumpStrength = 5;
        slidingDecelaration = -0.002f;
        crouchSpeed = 1.5f;
        jumpFixMaxHeight = 0.8f;
        curSpeed = 0;
        maxSpeed = 1.5f;
        walkSpeed = 2f;
        decalage = 0.07f;
    }

    void Awake()
    {
        crouchSize = new Vector2(0.18f, 0.08f);
        idleSize = new Vector2(0.18f, 0.29f);
        slidingVelocity = 0;
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        groundChecker = GroundDetector();

        IsGrounded();        
        Walk();
        anim.SetFloat("Speed",Mathf.Abs(walkingVelocity));

        //Run Test
        if (Input.GetAxis("Horizontal") != 0 && (runKey || run2Key) && !isCrouching) isRunning = true;
        else
        {
            isRunning = false;
            accelerationTime = 0f;
        }

        Run();
        SpriteFlip();

        //Enter Jump state
        if (jumpKey && isJumping == false && IsGrounded() && !isCrouching)
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

        grip.WallGripProcess();
        StandStill();

        if (!isJumping && !slideCooldown)
        {
            Crouch();
            Slide();
        }        

        CharacterSizeCheck();

        //Horizontal Speed Calculation
        if (!isPushing)
        {
            horizontalSpeed = (walkingVelocity + runningVelocity + slidingVelocity + movingPlatformXVelocity * 63f) * velocityMultiplicator * wallCollision.wallContact;
        }
        else
        {
            horizontalSpeed = push.pushVelocity;
        }
        playerMove = new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
        transform.Translate(playerMove, Space.World);
    }

    //Crouch
    private void Crouch()
    {
        if (crouchKeyDown && IsGrounded() && !isRunning) //Freeze de début d'accroupissement 
        {
            MoveFreeze(0.2f);
            waitBeforeStand = true;
            isCrouching = true;
            anim.SetBool("is crouching",true);
            StartCoroutine("Crouch2StandCooldown");
        }
        else if (crouchKeyDown && IsGrounded() && isRunning) //Lancement du Slide
        {
            waitBeforeStand = true;
            isSliding = true;
            anim.SetBool("is sliding", true);
        }
        else if ((!crouchKey || crouchKeyUp) && IsGrounded() && !isRunning && isCrouching) //Relâchement bouton pendant accroupissement
        {
            if (!waitBeforeStand && !CanStandOrNot()) //Si lâcher bouton après fin waitBeforeStand
            {
                MoveFreeze(0.1f);
                isCrouching = false;
                anim.SetBool("is crouching", false);
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
                SlideEndCooldown();
            }
            else
            {
                waitBeforeStand = false;
                Crouch2StandCooldown();
            }
        }
        else
        {

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
    public RaycastHit2D CanStandOrNot()
    {
        RaycastHit2D raycasthit = Physics2D.Raycast(new Vector2(boxCollider2d.bounds.center.x, boxCollider2d.bounds.center.y + boxCollider2d.bounds.extents.y + 0.02f),
            Vector2.up, 0.16f, platformLayerMask);

        return raycasthit;
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
            if (CanStandOrNot())
            {
                StopCoroutine("SlideDeceleration");
                isSliding = false;
                anim.SetBool("is sliding", false);
                isRunning = false;
                isCrouching = true;
                anim.SetBool("is crouching", true);
                slidingVelocity = 0f;
                velocityMultiplicator = 1f;

                SlideEndCooldown();
            }
            else
            {
                StopCoroutine("SlideDeceleration");
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
                slidingVelocity = 0f;
                velocityMultiplicator = 1f;
                isSliding = false;
                anim.SetBool("is sliding", false);

                SlideEndCooldown();
            }
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
    void SlideEndCooldown()
    {
        slideCooldown = true;
        StartCoroutine("SlideCooldown");
    }
    IEnumerator SlideCooldown()
    {
        yield return new WaitForSeconds(slideCooldownDuration);
        slideCooldown = false;
    }

    //Size check
    void CharacterSizeCheck()
    {
        if (!isCrouching && !isSliding)
        {
            dontFall.enabled = true;
            boxCollider2d.size = idleSize;
            spriteRenderer.sprite = idleSprite;
        }
        else if (isCrouching && !crouchKey && !CanStandOrNot())
        {            
            boxCollider2d.size = idleSize;
            spriteRenderer.sprite = idleSprite;
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
            isCrouching = false;
            anim.SetBool("is crouching", false);
        }
        else if (isSliding || isCrouching)
        {
            dontFall.enabled = false;
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(boxCollider2d.bounds.center.x, boxCollider2d.bounds.center.y - (boxCollider2d.bounds.extents.y * 0.78f)),
            new Vector2(boxCollider2d.bounds.extents.x - decalage, (boxCollider2d.bounds.extents.y* 1/12)),
            0f, Vector2.down, 0.05f, platformLayerMask);

        //RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(boxCollider2d.bounds.center.x, boxCollider2d.bounds.center.y - (boxCollider2d.bounds.extents.y * 0.78f)),
        //new Vector2(boxCollider2d.bounds.extents.x - decalage, (boxCollider2d.bounds.extents.y * 1 / 12)),
        //0f, Vector2.down, 0.05f, platformLayerMask);

        Color rayColor;
        rayColor = Color.green;
        
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
            verticalSpeed = movingPlatformYVelocity * 63f;
        }
        else if (!IsGrounded())
        {
            if (isJumping || grip.isClimbing)
            {
                if (jumpGravityAllowed)
                {
                    verticalSpeed -= gravity * Time.deltaTime;
                }
                else { verticalSpeed = 0; }
            }
            else 
            { 
                verticalSpeed -= gravity * Time.deltaTime; 
            }                     
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
        anim.SetBool("is jumping", true);
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
        yield return new WaitForSeconds(0.001f);
        if (isJumping == true && jumpInputMaintain == true && 
            headCollision.headHasTouched == false)
        {
            Vector3 arise = new Vector3(0,jumpStrength * Time.deltaTime, 0);
            transform.Translate(arise);

            StartCoroutine("JumpMovement");
        }
        else if (isJumping == true && (jumpInputMaintain == false || highestPointReached == true || headCollision.headHasTouched))
        {
            yield return new WaitForSeconds(glideTime);
            highestPointReached = false;
            StartCoroutine("JumpFall");
        }
    }
    IEnumerator JumpFall()
    {
        yield return new WaitForSeconds(0.01f);
        if (!IsGrounded() && isJumping == true && !grip.isClimbing)
        {
            jumpGravityAllowed = true;
            StartCoroutine("JumpFall");
        }
        else if (IsGrounded() && isJumping == true)
        {
            isJumping = false;
            anim.SetBool("is jumping", false);
            jumpGravityAllowed = false;
        }
    }
}