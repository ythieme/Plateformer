using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuggerGrab : MonoBehaviour
{
    Controler_YT controler;
    FearScript_FC fear;
    Transform player;
    SpriteRenderer playerSP;
    public Transform hugger;

    public int damage;
    public float cooldownDuration;
    public float power;
    public float escapeThreshold;
    public float damageInterval;
    float escapeValue;
    [System.NonSerialized] public int goingLeft;
    [System.NonSerialized] public bool detecting;
    bool trapped;
    bool inInterval;
    bool giveDamage;

    void Start()
    {
        controler = GameObject.FindGameObjectWithTag("Player").GetComponent<Controler_YT>();
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerSP = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        goingLeft = -1;
        escapeValue = 0;
    }
    
    void Update()
    {
        GoingLeft();
        if (trapped)
        {
            Escape();
            if (!inInterval)
            {
                StartCoroutine(Damage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !(fear.noDamage))
        {
            controler.enabled = false;
            trapped = true;
            Escape();
        }
    }

    void Escape()
    {
        if (escapeThreshold > escapeValue)
        {
            if(Input.GetButtonDown("Q") && escapeValue < escapeThreshold)
            {
                escapeValue += 1;
                if (!playerSP.flipX) playerSP.flipX = true;
            }
            else if (Input.GetButtonDown("D") && escapeValue < escapeThreshold)
            {
                escapeValue += 1;
                if (playerSP.flipX) playerSP.flipX = false;
            }
        }
        else if (escapeThreshold == escapeValue || escapeThreshold < escapeValue)
        {
            escapeValue = 0;
            fear.Knockback(power,goingLeft);
            controler.enabled = true;
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(damageInterval);
        inInterval = false;
        fear.fear--;
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
