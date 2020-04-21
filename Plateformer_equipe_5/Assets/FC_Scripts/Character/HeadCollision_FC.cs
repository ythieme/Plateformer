using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision_FC : MonoBehaviour
{
        public BoxCollider2D character;
    
    public LayerMask platformLayerMask;
    Vector2 lateralDetectorOrigin;
    public float lateralDetectorLength;
    public float decalage;

    public Controler_YT movement;
    public bool headHasTouched;

    void Start()
    {
        decalage = 0.02f;
        lateralDetectorLength = 0;
        headHasTouched = false;
    }
        
    void Update()
    {          
        lateralDetectorLength = character.size.x - decalage;
        
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (character.bounds.extents.x),
            character.bounds.center.y + character.bounds.extents.y + decalage);

        CeilingContact();
        Debug.DrawRay(lateralDetectorOrigin,
            Vector2.left * (lateralDetectorLength * 2), Color.blue);
    }

    public RaycastHit2D TopDetector()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(lateralDetectorOrigin,
            Vector2.left, lateralDetectorLength, platformLayerMask);        

        return raycastHit;
    }

    public void CeilingContact()
    {
        if(TopDetector() && movement.isJumping)
        {
            headHasTouched = true;                         
        }
        else
        {
            headHasTouched = false;
        }
    }
}
