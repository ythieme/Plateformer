using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FearScript_FC : MonoBehaviour
{
    public Controler_YT controler;
    public PlayerCheckpointManager checkpoint;
    public HealthBarScript_FC healthBar;
    

    public GameObject character;
    public Transform position;

    [Header("Animation")]
    public Animator anim;
    public Animator deathtransition;
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
        healthBar.SetMaxHealth(maxfear);
        character = GameObject.FindGameObjectWithTag("Player");
        controler = character.GetComponent<Controler_YT>();
        anim = character.GetComponent<Animator>();
        checkpoint = character.GetComponentInChildren<PlayerCheckpointManager>();
    }
    private void Update()
    {
        position = character.transform;
        anim.SetInteger("Fear", fear);
        healthBar.SetHealth(fear);
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
        anim.SetBool("is jumping", false);
        controler.enabled = false;
        deathtransition.SetTrigger("StartFade");
        yield return new WaitForSeconds(2);
        deathtransition.SetTrigger("EndFade");
        isDead = true;
        yield return new WaitForSeconds(1);
        deathtransition.ResetTrigger("EndFade");
        deathtransition.ResetTrigger("StartFade");
    }
    
    public void DealDamage(int damageValue)
    {
        if (!noDamage)
        {
            fear -= damageValue;
            CameraShaker.Instance.ShakeOnce(8f, 6f, 0.5f, 0.5f);
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