using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemy : MonoBehaviour
{
    public FearScript_FC fear;

    public Transform player;
    Vector2 startPosition;
    public EnemyDamageProcess damage;
    public bool detecting;

    public float speed;    
    float step;   

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        startPosition = transform.position;
        damage = GetComponent<EnemyDamageProcess>();
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
    }
    void Update()
    {
        step = speed * Time.deltaTime;
        if (fear.fear < 0 || fear.fear == 0)
        {
            transform.Translate(new Vector3(startPosition.x - transform.position.x, startPosition.y - transform.position.y));
        }
    }

    public void EnemyMoveStart()
    {
        if (!(fear.fear < 0 || fear.fear == 0))
        {
            StartCoroutine(MoveTowardPlayer());
        }            
    }

    IEnumerator MoveTowardPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        if (!(fear.fear < 0 || fear.fear == 0))
        {
            if (detecting)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, step);
                StartCoroutine(MoveTowardPlayer());
            }        
        } 
    }    
}
