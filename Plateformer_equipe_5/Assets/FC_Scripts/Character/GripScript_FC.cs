using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripScript_FC : MonoBehaviour
{
    public BoxCollider2D character;

    public LayerMask grabablePlatforms;
    Vector2 lateralDetectorOrigin;
    public float lateralDetectorLength;
    public float decalageX;
    public float decalageY;

    public Controler_YT movement;

    int goingLeft;
    public bool isClimbing;

    //Grip Parameters
    [SerializeField]
    public float gripJump;
    public float gripForward;

    void Start()
    {
        isClimbing = false;
        decalageY = 0.04f;
        decalageX = 0.01f;
        goingLeft = -1;
        lateralDetectorLength = 0;
        gripJump = 3f;
        gripForward = 1f;
    }

    public void WallGripProcess()
    {
        lateralDetectorLength = character.size.y - decalageY;

        GoingLeft();
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (decalageX * goingLeft) + (character.bounds.extents.x) * goingLeft,
            character.bounds.center.y - character.bounds.extents.y + decalageY);

        Debug.DrawRay(lateralDetectorOrigin,
            Vector2.up * (lateralDetectorLength * 2), Color.red);

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
        RaycastHit2D raycastHit = Physics2D.Raycast(lateralDetectorOrigin,
            Vector2.up, lateralDetectorLength, grabablePlatforms);

        return raycastHit;
    }

    public void WallGrip()
    {
        if (LateralDetector() && isClimbing == false)
        {
            isClimbing = true;

            StartCoroutine("GetOnThePlatform");
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
        }
    }        
}
