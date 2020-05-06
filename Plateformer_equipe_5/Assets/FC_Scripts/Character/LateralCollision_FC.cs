using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralCollision_FC : MonoBehaviour
{
    public BoxCollider2D character;
    
    public LayerMask platformLayerMask;
    Vector2 lateralDetectorOrigin;
    public float lateralDetectorLength;
    public float decalage;

    public Controler_YT movement;

    int goingLeft;
    [System.NonSerialized] public int wallContact;

    void Start()
    {
        character = GetComponent<BoxCollider2D>();
        movement = GetComponent<Controler_YT>();
        decalage = 0.08f;
        goingLeft = -1;
        lateralDetectorLength = 0;
    }
        
    void Update()
    {          
        GoingLeft();
        lateralDetectorLength = character.size.y - decalage/2;
        
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (character.bounds.extents.x)* goingLeft,
            character.bounds.center.y - character.bounds.extents.y + decalage);
        
        WallContact();        

        Debug.DrawLine(new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y),
            new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y + lateralDetectorLength), Color.blue);
    }

    int GoingLeft()
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
            new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y + lateralDetectorLength), platformLayerMask);
        
        return raycastHit;
    }

    void WallContact()
    {
        if(LateralDetector())
        {
            wallContact = 0;                          
        }
        else
        {
            wallContact = 1;
        }
    }
}
