using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript_FC : MonoBehaviour
{
    public Controler_YT controler;

    RaycastHit2D playerDetector;
    public LayerMask playerLayerMask;
    public BoxCollider2D boxcollider;

    float decalageY = 0.05f;

    float originX;
    float originY;

    float firstXPosition;
    float secondXPosition;
    float firstYPosition;
    float secondYPosition;
    [System.NonSerialized] public float actualSpeedX;
    [System.NonSerialized] public float actualSpeedY;

    void Start()
    {
        StartCoroutine(ActualSpeed());
    }

    void Update()
    {
        originX = (boxcollider.bounds.center.x - boxcollider.bounds.extents.x);
        originY = (boxcollider.bounds.center.y + boxcollider.bounds.extents.y + decalageY);
                
        PlayerDetector();
        StickThePlayer();
    }

    void StickThePlayer()
    {
        if (PlayerDetector())
        {
            controler.movingPlatformXVelocity = actualSpeedX;
            controler.movingPlatformYVelocity = actualSpeedY;
        }
        else
        {
            controler.movingPlatformXVelocity = 0f;
            controler.movingPlatformYVelocity = 0f;
        }
    }

    IEnumerator ActualSpeed()
    {
        new WaitForSeconds(0.01f);
        firstXPosition = transform.position.x;
        firstYPosition = transform.position.y;

        yield return new WaitForEndOfFrame();

        secondXPosition = transform.position.x;
        secondYPosition = transform.position.y;

        actualSpeedX = (secondXPosition - firstXPosition) * 175;
        actualSpeedY = (secondYPosition - firstYPosition) * 175;
        StartCoroutine(ActualSpeed());
    }

    public RaycastHit2D PlayerDetector()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2 (originX, originY),
            Vector2.right, boxcollider.bounds.size.x,  playerLayerMask );

        Debug.DrawRay(new Vector2(originX, originY),
            transform.TransformDirection(new Vector2(boxcollider.bounds.size.x, 0)), Color.blue);

        return raycastHit;
    }
}
