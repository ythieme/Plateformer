using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Push : MonoBehaviour

{

    private Controler_FC controler_fc;


    bool isWalking;
    bool ispushing;
    bool iscloseobject;
    Rigidbody2D rigidbody2d;
    
    [SerializeField]
    LayerMask platformLayerMask;
    BoxCollider2D boxCollider2d;
    Animator animator;
    public float distance = 1f;

    private void Awake()
    {
        controler_fc = GetComponent<Controler_FC>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        walk();
        push();

        if (isWalking == true && Input.GetKeyDown(KeyCode.A) && iscloseobject == true)

        {
            ispushing = true;
        }

        else
        {
            ispushing = false;
        }
    }

    private float push()
    {


        return 0f;
    }

    private float walk()
    {
        isWalking = true;

        return 0f;
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


}

