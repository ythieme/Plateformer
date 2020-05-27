using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageProcess : MonoBehaviour
{
    public FearScript_FC fear;
    public Transform player;

    public int damage;    
    public float cooldownDuration;
    public float power;
    [System.NonSerialized] public int goingLeft;
    [System.NonSerialized] public bool detecting;
    [System.NonSerialized] public bool inCooldown;

    void Start()
    {
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if ((player.transform.position.x - transform.position.x) < 0) goingLeft = -1;
        else if ((player.transform.position.x - transform.position.x) > 0) goingLeft = 1;

        inCooldown = false;
        detecting = false;
    }
    private void Update()
    {
        GoingLeft();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!inCooldown && other.gameObject.CompareTag("Player") && !(fear.noDamage))
        {
            fear.DealDamage(damage);
            fear.Knockback(power, goingLeft);
            inCooldown = true;
            StartCoroutine(DamageDealingCooldown());
        }
    }
    IEnumerator DamageDealingCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        inCooldown = false;
    }
    public int GoingLeft()
    {
        if ((player.transform.position.x - transform.position.x) < 0 && goingLeft == 1)
        {
            goingLeft = -1;
        }
        else if ((player.transform.position.x - transform.position.x) > 0 && goingLeft == -1)
        {
            goingLeft = 1;
        }
        else
        { }

        return goingLeft;
    }
}
