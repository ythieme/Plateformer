using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearScript_FC : MonoBehaviour
{
    public int maxfear;
    public int fear;
    [System.NonSerialized] public bool isDead;

    void FixedUpdate()
    {
        DeathCheck();
    }

    public bool DeathCheck()
    {
        if (fear <= 0 || fear == 0)
        {
            StartCoroutine(DeadState());            
        }
        else if (fear > 0)
        {
            isDead = false;
        }
        return isDead;
    }

    IEnumerator DeadState()
    {
        isDead = true;
        yield return new WaitForSeconds(0.1f);
        fear = maxfear;
    }
    
    public void DealDamage(int damageValue)
    {
        fear -= damageValue;
    }
}
