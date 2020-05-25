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
        }
    }

}
