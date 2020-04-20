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

    public LateralCollision_FC wallCollision;
    public HeadCollision_FC headCollision;
    public GripScript_FC grip;

    [System.NonSerialized] public bool jumpKey, jumpKeyDown, jumpKeyUp, runKey, runKeyDown, runKeyUp,
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

    //Slide parameters
    [System.NonSerialized] public bool isSliding;
    [SerializeField] float slidingDecelaration;
    float slidingVelocity;
    bool slideCooldown;
    float slideCooldownDuration;

    //GoundCheck Composents
    [Header ("GroundCheck Composents")]
    [SerializeField] public LayerMask platformLayerMask;
    [SerializeField] public BoxCollider2D boxCollider2d;
    [SerializeField] public float gravity;
    [SerializeField] public float decalage;

    //Move parameters
    float walkingVelocity;
    float runningVelocity;
    float velocityMultiplicator = 1;
    [System.NonSerialized] public float movingPlatformXVelocity;
    [System.NonSerialized] public float movingPlatformYVelocity;
    [System.NonSerialized] public bool isRunning;
    [System.NonSerialized] public bool moveFreeze;

    //Run parameters    
    [Header("Move Parameters")]
    [SerializeField]
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    private float curSpeed;
    float accelerationTime;
    [SerializeField] public float accelerationTimeEnd;
    [SerializeField] float walkSpeed;

    //Variables Jump        
    [Header("Jump")]
    [SerializeField] float jumpFixMaxHeight;
    [SerializeField] float jumpStrength;
    [SerializeField] float glideTime;
    [System.NonSerialized] public bool highestPointReached, isJumping, jumpGravityAllowed;

    bool jumpInputMaintain;

    float jumpHeight;
    float jumpMaxHeight;

    void Start()
    {
        movingPlatformXVelocity = 0f;
        gravity = 20f;
        acceleration = 0.5f;
        deceleration = -0.01f;
        glideTime = 0.1f;
        jumpStrength = 10;
        slidingDecelaration = -0.002f;
        slideCooldownDuration = 0.15f;
        crouchSpeed = 1.5f;
        jumpFixMaxHeight = 0.8f;
        curSpeed = 0;
        maxSpeed = 1.5f;
        walkSpeed = 2f;
        decalage = 0.07f;
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

        IsGrounded();        
        Walk();

        //Run Test
        if (Input.GetAxis("Horizontal") != 0 && runKey && !isCrouching) isRunning = true;
        else
        {
            isRunning = false;
            accelerationTime = 0f;
        }

        Run();
        SpriteFlip();

        //Enter Jump state
        if (jumpKeyDown && isJumping == false && IsGrounded() && !isCrouching)
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
        horizontalSpeed = (walkingVelocity + runningVelocity + slidingVelocity + movingPlatformXVelocity) * velocityMultiplicator * wallCollision.wallContact;
        Debug.Log(movingPlatformYVelocity);
        Debug.Log(movingPlatformXVelocity);

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
            StartCoroutine("Crouch2StandCooldown");
        }
        else if (crouchKeyDown && IsGrounded() && isRunning) //Lancement du Slide
        {
            waitBeforeStand = true;
            isSliding = true;
        }
        else if ((!crouchKey || crouchKeyUp) && IsGrounded() && !isRunning && isCrouching) //Relâchement bouton pendant accroupissement
        {
            if (!waitBeforeStand && !CanStandOrNot()) //Si lâcher bouton après fin waitBeforeStand
            {
                MoveFreeze(0.1f);
                isCrouching = false;
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
                SlideEndCooldown();
            }
            else
            {

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
        RaycastHit2D raycasthit = Physics2D.Raycast(new Vector2(boxCollider2d.bounds.center.x, boxCollider2d.bounds.center.y + boxCollider2d.bounds.extents.y),
            Vector2.up, 0.16f, platformLayerMask );

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
                isRunning = false;
                isCrouching = true;
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
            boxCollider2d.size = idleSize;
            spriteRenderer.sprite = idleSprite;
        }
        else if (isCrouching && !crouchKey && !CanStandOrNot())
        {
            boxCollider2d.size = idleSize;
            spriteRenderer.sprite = idleSprite;
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 0.08f);
            isCrouching = false;
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(boxCollider2d.bounds.center.x, boxCollider2d.bounds.center.y - (boxCollider2d.bounds.extents.y * 5/6)),
            new Vector2(boxCollider2d.bounds.extents.x - decalage, (boxCollider2d.bounds.extents.y* 1/12)),
            0f, Vector2.down, 0.05f, platformLayerMask);

        Color rayColor;
        rayColor = Color.green;
        /*Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x - decalage, 0),
            Vector2.down * (boxCollider2d.bounds.extents.y), rayColor); //gauche

        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x - decalage, 0),
            Vector2.down * (boxCollider2d.bounds.extents.y), rayColor); //droite

        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x - decalage, (boxCollider2d.bounds.extents.y * 1/3)),
            Vector2.right * (boxCollider2d.bounds.extents.x), rayColor); //bas

        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x - decalage, - (boxCollider2d.bounds.extents.y * 2/3)),
            Vector2.left * (boxCollider2d.bounds.extents.x), rayColor); //haut
            */

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
            verticalSpeed = movingPlatformYVelocity;
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
        if (isJumping == true && jumpInputMaintain == true && 
            headCollision.headHasTouched == false)
        {
            Vector3 arise =
                new Vector3(0, playerInput.y * jumpStrength, 0);
            transform.localPosition += arise * Time.deltaTime;

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
            jumpGravityAllowed = false;
        }
    }
}