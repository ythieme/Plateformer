using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemyPlayerDetector : MonoBehaviour
{
    public FlyingEnnemy behaviour;

    private void Start()
    {

    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            behaviour.detecting = true;
            behaviour.EnemyMoveStart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            behaviour.detecting = false;
            StartCoroutine(MoveTowardSpawn());
        }
    }

    IEnumerator MoveTowardSpawn()
    {
        yield return new WaitForSeconds(0.01f);
        if (!behaviour.detecting)
        {
            behaviour.transform.position = Vector2.MoveTowards(behaviour.transform.position, behaviour.startPosition, behaviour.step / 2);
            StartCoroutine(MoveTowardSpawn());
        }
    }
}
