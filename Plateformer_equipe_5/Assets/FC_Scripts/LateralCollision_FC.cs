using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralCollision_FC : MonoBehaviour
{
    public BoxCollider2D character;
    
    public LayerMask platformLayerMask;
    RaycastHit2D lateralDetector;
    Vector2 lateralDetectorOrigin;
    public float lateralDetectorLength;
    public float decalage;

    public Controler_YT movement;

    int goingLeft;
    [System.NonSerialized]
    public int wallContact;

    void Start()
    {
        decalage = 0.05f;
        goingLeft = -1;
        lateralDetectorLength = 0;
    }
        
    void Update()
    {                
        lateralDetectorLength = character.size.y - decalage;
        
        GoingLeft();        
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (character.bounds.extents.x)* goingLeft, character.bounds.center.y - character.bounds.extents.y + decalage);
                      
        WallContact();
        Debug.DrawRay(lateralDetectorOrigin,
            Vector2.up * (lateralDetectorLength * 2), Color.blue);
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
        RaycastHit2D raycastHit = Physics2D.Raycast(lateralDetectorOrigin,
            Vector2.up, lateralDetectorLength, platformLayerMask);        

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
