using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush_YT : MonoBehaviour

{
    bool isWalking;
    bool ispushing;
    bool iscloseobject;
    GameObject box;
    
    [SerializeField]
    LayerMask boxMask;
    BoxCollider2D boxCollider2d;
    Animator animator;
    public float distance = 1f;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {

        if (isWalking == true && Input.GetKeyDown(KeyCode.E) && iscloseobject == true)

        {
            ispushing = true;
        }

        else
        {
            ispushing = false;
        }

        Physics2D.queriesStartInColliders = true;
        RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        if (hit.collider != null && Input.GetKeyDown(KeyCode.E))
        {
            /*
            box = hit.collider.gameObject;
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<FixedJoint2D>().enabled = this.GetComponent<BoxCollider2D>();
            */
        }
    }

    void OndrawGizmos()
    {
        /*
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
        */

    }

    /*
    private float push()
    {


        return 0f;
    }

    private float Walk()
    {
        isWalking = true;

        return 0f;
    }
  */

}

