using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPush_FC : MonoBehaviour
{
    [System.NonSerialized] public bool pushKey, pushKeyUp, pushKeyDown;
    int direction = -1;

    public LayerMask boxLayerMask;
    public float distance;
    public float distance2;
    public float pushVelocity;
    public float pushSpeed;
    public Controler_YT controler;
    public PushableBox pushableBox;

    public GameObject box;
    public Animator anim;
    RaycastHit2D hit;
    RaycastHit2D pushRange;

    private void Start()
    {
        distance = 0.13f;
        distance2 = 0.18f;
        controler = GetComponent<Controler_YT>();
        pushVelocity = Input.GetAxis("Horizontal") * pushSpeed * Time.deltaTime;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        pushVelocity = Input.GetAxis("Horizontal") * pushSpeed * Time.deltaTime;
        PushProcess();
    }

    public void PushProcess()
    {
        RayFlip();

        if (pushKey)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right * direction, distance2, boxLayerMask);
            pushRange = Physics2D.Raycast(transform.position, Vector2.right * direction, distance, boxLayerMask);
            Debug.DrawRay(transform.position, Vector2.right * direction * distance);
            Debug.DrawRay(transform.position, Vector2.right * direction * distance2);
        }

        if (hit.collider != null && pushRange.collider != null && pushKey && controler.IsGrounded())
        {
            box = hit.collider.gameObject;
            pushableBox = box.GetComponent<PushableBox>();
            if (!pushableBox.isBesideWall)
            {
                box.GetComponent<Transform>().Translate(new Vector2(pushVelocity, 0), Space.World);
                controler.isPushing = true;
            }
        }
        else
        {
            controler.isPushing = false;
        }

        if (pushKey && hit.collider != null && controler.IsGrounded())
        {
            anim.SetBool("is Pushing", true);
        }
        else
        {
            anim.SetBool("is Pushing", false);
        }
    }    

    void RayFlip()
    {
        if (Input.GetAxis("Horizontal") > 0 )
        {
            direction = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            direction = -1;
        }
        else { }
    }
}
