using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterCollisionFlags
{
    public bool left, right, above, below;

    public void Reset()
    {
        left = right = above = below = false;
    }
}

public class CollisionDetector_FC : MonoBehaviour
{
    public float skinWidthMultiplier = 0.95f;
    public Transform self;
    public BoxCollider2D box;
    public LayerMask layerMask;

    public CharacterCollisionFlags flags;

    void Start()
    {
        flags.Reset();
    }

    public float CastBoxHorizontal(float distance)
    {
        RaycastHit2D result = Physics2D.BoxCast(
            self.position,
            new Vector2(box.size.x * self.lossyScale.x * skinWidthMultiplier, box.size.y * self.lossyScale.y * skinWidthMultiplier),
            0,
            Vector2.right * Mathf.Sign(distance),
            Mathf.Abs(distance),
            layerMask);

        if (result.collider != null)
        {
            float startPoint = self.position.x + (box.size.x * self.lossyScale.x * 0.5f * Mathf.Sign(distance));
            float newDistance = Mathf.Sign(distance) * Mathf.Abs(result.point.x - startPoint);

            if (distance < 0)
                flags.left = true;
            if (distance > 0)
                flags.right = true;

            //EMP : I add the test on right/left flags to solve issue on collision.
            if (flags.left || flags.right)
                return 0f;
            else
                return newDistance;
        }

        if (distance < 0)
            flags.left = false;
        if (distance > 0)
            flags.right = false;

        return distance;
    }

    public float CastBoxVertical(float distance)
    {
        RaycastHit2D result = Physics2D.BoxCast(
            self.position,
            new Vector2(box.size.x * self.lossyScale.x * skinWidthMultiplier,
                box.size.y * self.lossyScale.y * skinWidthMultiplier),
            0,
            Vector2.up * Mathf.Sign(distance),
            Mathf.Abs(distance),
            layerMask);

        if (result.collider != null)
        {
            float startPoint = self.position.y + (box.size.y * self.lossyScale.y * 0.5f * Mathf.Sign(distance));
            float newDistance = Mathf.Sign(distance) * Mathf.Abs(result.point.y - startPoint);

            if (distance < 0)
                flags.below = true;
            if (distance > 0)
                flags.above = true;

            //EMP : I add the test on above flag to check if we can jump or not
            // throw a platform. Just return 0 as no distance ;)
            if (flags.above)
                return 0f;
            else
                return newDistance;
        }

        if (distance < 0) flags.below = false;
        if (distance > 0) flags.above = false;

        return distance;
    }
}