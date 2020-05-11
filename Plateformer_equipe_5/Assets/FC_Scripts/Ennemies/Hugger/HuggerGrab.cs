using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuggerGrab : MonoBehaviour
{
    Controler_YT controler;
    FearScript_FC fear;
    Transform player;
    SpriteRenderer playerSP;
    MovingPlatformScript_FC move;
    public Transform hugger;

    public int damage;
    public float power;
    public float escapeThreshold;
    public float damageInterval;
    float escapeValue;
    [System.NonSerialized] public int goingLeft;
    [System.NonSerialized] public bool detecting;
    bool trapped;
    bool inInterval;

    void Start()
    {
        move = GetComponentInParent<MovingPlatformScript_FC>();
        controler = GameObject.FindGameObjectWithTag("Player").GetComponent<Controler_YT>();
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerSP = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        goingLeft = -1;
        escapeValue = 0;
        inInterval = false;
    }
    
    void Update()
    {
        GoingLeft();
        if (trapped)
        {
            Escape();
            if (!inInterval)
            {
                StartCoroutine(Interval());
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
            move.hugging = true;
            Escape();
        }
    }

    void Escape()
    {
        if (escapeThreshold > escapeValue)
        {
            if(Input.GetAxis("Horizontal") < 0 && escapeValue < escapeThreshold)
            {
                escapeValue += 1;
                if (!playerSP.flipX) playerSP.flipX = true;
            }
            else if (Input.GetAxis("Horizontal") > 0 && escapeValue < escapeThreshold)
            {
                escapeValue += 1;
                if (playerSP.flipX) playerSP.flipX = false;
            }
        }
        else if (escapeThreshold == escapeValue || escapeThreshold < escapeValue)
        {
            trapped = false;
            escapeValue = 0;
            fear.Knockback(power,goingLeft);
            controler.enabled = true;
            move.hugging = false;
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.01f);
        fear.fear--;
    }
    
    IEnumerator Interval()
    {
        if(!inInterval)
        {   
            inInterval = true;
            yield return new WaitForSeconds(damageInterval);
            inInterval = false;
        }        
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
