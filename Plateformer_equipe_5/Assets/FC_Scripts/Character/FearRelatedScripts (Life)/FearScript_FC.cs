using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearScript_FC : MonoBehaviour
{
    public int fear;
    [System.NonSerialized] public bool isDead;
    
    void FixedUpdate()
    {
        DeathCheck();
    }

    public bool DeathCheck()
    {
        if (fear <= 0)
        {
            isDead = true;
        }
        else if (fear > 0)
        {
            isDead = false;
        }
        return isDead;
    }

    public void DealDamage(int damageValue)
    {
        fear -= damageValue;
    }
}
