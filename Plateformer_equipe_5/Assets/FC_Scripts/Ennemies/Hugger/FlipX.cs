using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipX : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public MovingPlatformScript_FC movingPlatform;

    void Update()
    {
        SpriteFlip();
    }

    void SpriteFlip()
    {
        if (movingPlatform.actualSpeedX > 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
        }
        else if (movingPlatform.actualSpeedX < 0 && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
        }
        else { }
    }
}
