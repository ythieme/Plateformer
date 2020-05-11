using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPush_FC : MonoBehaviour
{
    [System.NonSerialized] public bool pushKey, pushKeyUp, pushKeyDown;
    int direction = -1;

    public LayerMask boxLayerMask;
    public float distance;
    public float pushVelocity;
    public float pushSpeed;
    public Controler_YT controler;
    public PushableBox pushableBox;

    public GameObject box;
    RaycastHit2D hit;

    private void Start()
    {
        distance = 0.23f;
        controler = GetComponent<Controler_YT>();
        pushVelocity = Input.GetAxis("Horizontal") * pushSpeed * Time.deltaTime;
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
            hit = Physics2D.Raycast(transform.position, Vector2.right * direction, distance, boxLayerMask);

            Debug.DrawRay(transform.position, Vector2.right * direction * distance);
        }

        if (hit.collider != null && pushKey && controler.IsGrounded())
        {
            box = hit.collider.gameObject;
            pushableBox = box.GetComponent<PushableBox>();
            if (!pushableBox.isBesideWall)
            {
                box.GetComponent<Transform>().Translate(new Vector2(pushVelocity * 1.001f, 0), Space.World);
                controler.isPushing = true;
            }            
        }
        else controler.isPushing = false;
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
