using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemy : MonoBehaviour
{
    public FearScript_FC fear;
    public Transform player;

    public float speed;
    public int damage;
    public float cooldownDuration;
    bool inCooldown;
    float step;    

    void Start()
    {
        StartCoroutine(MoveTowardPlayer());
        inCooldown = false;
    }

    void Update()
    {
        step = speed * Time.deltaTime;
    }

    IEnumerator MoveTowardPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        transform.position = Vector2.MoveTowards(transform.localPosition, player.position, step);
        StartCoroutine(MoveTowardPlayer());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inCooldown && other.gameObject.CompareTag("Player"))
        {
            fear.DealDamage(damage);
            inCooldown = true;
            StartCoroutine(DamageDealingCooldown());
        }
    }

    IEnumerator DamageDealingCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        inCooldown = false;
    }
}
