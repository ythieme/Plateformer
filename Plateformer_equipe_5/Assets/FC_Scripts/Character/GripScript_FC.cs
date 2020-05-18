using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripScript_FC : MonoBehaviour
{
    public BoxCollider2D character;
    public Animator anim;
    public AnimationCurve riseCurve;

    public LayerMask grabablePlatforms;
    Vector2 lateralDetectorOrigin;
    public float lateralDetectorLength;
    public float decalageX = 0.02f;
    public float decalageY;

    public Controler_YT movement;

    int goingLeft;
    public bool isClimbing;
    bool animating;

    //Grip Parameters
    [SerializeField]
    public float gripJump = 5f;
    public float gripForward = 2f;

    float coroutineCount;

    void Start()
    {
        anim = GetComponent<Animator>();
        isClimbing = false;
        decalageY = 0.06f;
        goingLeft = -1;
        lateralDetectorLength = 0;
    }

    public void WallGripProcess()
    {
        lateralDetectorLength = character.size.y - decalageY * 2;

        GoingLeft();
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (decalageX * goingLeft) + (character.bounds.extents.x) * goingLeft,
            character.bounds.center.y - character.bounds.extents.y + decalageY);

        Debug.DrawLine(new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y),
            new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y + lateralDetectorLength), Color.red);

        WallGrip();
    }

    public int GoingLeft()
    {
        if (Input.GetAxis("Horizontal") < 0 && goingLeft == 1)
        {
            goingLeft = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0 && goingLeft == -1)
        {
            goingLeft = 1;
        }
        else
        { }

        return goingLeft;
    }

    public RaycastHit2D LateralDetector()
    {
        RaycastHit2D raycastHit = Physics2D.Linecast(new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y),
            new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y + lateralDetectorLength), grabablePlatforms);

        return raycastHit;
    }

    public void WallGrip()
    {
        if (LateralDetector() && isClimbing == false && movement.isJumping && !movement.IsGrounded())
        {
            isClimbing = true;
            movement.enabled = false;
            movement.enabled = true;
            if (!animating) StartCoroutine(Anim());
            StartCoroutine("GetOnThePlatform2");
        }
        else if (!LateralDetector() && isClimbing == true)
        {
            isClimbing = false;
        }
        else
        {
            
        }

        if (isClimbing && movement.jumpGravityAllowed && !(movement.IsGrounded()))
        {
            movement.jumpGravityAllowed = false;
            movement.StopCoroutine("JumpFall");
        }
    }

    IEnumerator Anim()
    {
        animating = true;
        anim.SetBool("is Climbing", true);
        anim.SetBool("is jumping", false);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("is Climbing", false);
        animating = false;
    }

    IEnumerator GetOnThePlatform()
    {
        yield return new WaitForSeconds(0.01f);
        if (LateralDetector() && isClimbing == true)
        {
            transform.Translate(new Vector2(0, gripJump) * Time.deltaTime);
            StartCoroutine("GetOnThePlatform");
        }
        else if (!LateralDetector() && isClimbing == true && !movement.IsGrounded())
        {
            transform.Translate(new Vector2(gripForward * goingLeft, 0) * Time.deltaTime);
            StartCoroutine("GetOnThePlatform");
        }       
        else 
        {
            movement.isJumping = false;
            anim.SetBool("is jumping", false);
        }
    }

    IEnumerator GetOnThePlatform2()
    {
        yield return new WaitForSeconds(0.01f);
        coroutineCount += 10 * Time.deltaTime;
        if (coroutineCount <= 1)
        {
            transform.Translate(new Vector2(riseCurve.Evaluate(coroutineCount) * gripForward * goingLeft, riseCurve.Evaluate(coroutineCount) * gripJump) * Time.deltaTime);
            StartCoroutine("GetOnThePlatform2");
        }        
        else
        {
            movement.isJumping = false;
            anim.SetBool("is jumping", false);
            coroutineCount = 0;
        }
    }
}
