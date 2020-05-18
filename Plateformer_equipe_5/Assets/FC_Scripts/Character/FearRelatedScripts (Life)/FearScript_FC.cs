using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearScript_FC : MonoBehaviour
{
    public Controler_YT controler;
    public PlayerCheckpointManager checkpoint;
    public GameObject character;
    public Transform position;

    [Header("Animation")]
    public Animator anim;

    public int maxfear;
    public int fear;
    public float noDamageTime;
    float coroutineCount;    

    public AnimationCurve knockCurve;

    [System.NonSerialized] public bool isDead;
    public bool knockbacked;
    public bool noDamage;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        controler = character.GetComponent<Controler_YT>();
        anim = character.GetComponent<Animator>();
        checkpoint = character.GetComponentInChildren<PlayerCheckpointManager>();
    }
    private void Update()
    {
        position = character.transform;
        anim.SetInteger("Fear", fear);
    }
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
        anim.SetBool("is jumping", false);
        controler.enabled = false;
        yield return new WaitForSeconds(1.5f);
        fear = maxfear;
        checkpoint.Death();
    }
    
    public void DealDamage(int damageValue)
    {
        if (!noDamage)
        {
            fear -= damageValue;
            anim.SetBool("is Hurted", true);
            StartCoroutine(AnimSetOff());
            StartCoroutine(InvincibilityFrames(noDamageTime));
        }        
    }

    IEnumerator AnimSetOff()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("is Hurted", false);
    }
    IEnumerator InvincibilityFrames(float noDamageTime)
    {
        noDamage = true;
        yield return new WaitForSeconds(noDamageTime);
        noDamage = false;
    }

    public void Knockback(float power, int goingLeft)
    {
        controler.velocityMultiplicator = 0.2f;
        knockbacked = true;
        StartCoroutine(KnockBackCooldown(power, goingLeft));
    }

    IEnumerator KnockBackCooldown(float power, int goingLeft)
    {
        yield return new WaitForSeconds(0.01f);
        coroutineCount += 10 * Time.deltaTime;
        if (knockbacked && coroutineCount<= 1)
        {            
            position.Translate(new Vector2(((knockCurve.Evaluate(coroutineCount) * power ) * goingLeft),
                 knockCurve.Evaluate(coroutineCount)* power) * Time.deltaTime);
            StartCoroutine(KnockBackCooldown(power, goingLeft));
        }
        else
        {
            knockbacked = false;
            controler.velocityMultiplicator = 1;
            coroutineCount = 0;
        }
    }
}