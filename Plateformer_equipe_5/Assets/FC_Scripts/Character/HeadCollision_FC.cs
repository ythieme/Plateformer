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
    public float decalageX;

    public Controler_YT movement;
    public bool headHasTouched;

    void Start()
    {
        decalage = 0.01f;
        decalageX = 0.045f;
        lateralDetectorLength = 0;
        headHasTouched = false;
    }
        
    void Update()
    {          
        lateralDetectorLength = character.size.x - decalageX *2;
        
        lateralDetectorOrigin = new Vector2(character.bounds.center.x + (character.bounds.extents.x) - decalageX,
            character.bounds.center.y + character.bounds.extents.y + decalage);

        CeilingContact();
        Debug.DrawLine(new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y),
             new Vector2(lateralDetectorOrigin.x - lateralDetectorLength, lateralDetectorOrigin.y), Color.blue);
    }

    public RaycastHit2D TopDetector()
    {
        RaycastHit2D raycastHit = Physics2D.Linecast(new Vector2(lateralDetectorOrigin.x, lateralDetectorOrigin.y),
             new Vector2(lateralDetectorOrigin.x - lateralDetectorLength, lateralDetectorOrigin.y), platformLayerMask);

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
