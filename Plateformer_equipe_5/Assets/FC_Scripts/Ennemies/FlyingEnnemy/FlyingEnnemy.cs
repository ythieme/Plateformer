using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemy : MonoBehaviour
{
    public Transform player;
    public EnemyDamageProcess damage;
    public bool detecting;

    public float speed;    
    float step;   

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        damage = GetComponent<EnemyDamageProcess>();
    }
    void Update()
    {
        step = speed * Time.deltaTime;
    }

    public void EnemyMoveStart()
    {
        StartCoroutine(MoveTowardPlayer());
    }

    IEnumerator MoveTowardPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        if (detecting)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, step);
            StartCoroutine(MoveTowardPlayer());
        }        
    }    
}
