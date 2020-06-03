using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlatforms : MonoBehaviour
{
    public FearScript_FC fearScript;
    public float damageInterval;
    bool inInterval;
    bool giveDamage;

    private void Awake()
    {
        inInterval = false;
    }
    
    private void Update()
    {
        if (!inInterval && giveDamage)
        {
            StartCoroutine(PlatformDamage());
            inInterval = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            giveDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            giveDamage = false;
            inInterval = false;
        }
    }

    IEnumerator PlatformDamage()
    {
        yield return new WaitForSeconds(damageInterval);
        if(giveDamage)
        {
            inInterval = false;
            fearScript.DealDamage(1);
        }
    }
}
