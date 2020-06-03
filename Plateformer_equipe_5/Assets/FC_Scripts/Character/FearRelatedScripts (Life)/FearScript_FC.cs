using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class FearScript_FC : MonoBehaviour
{
    public Controler_YT controler;
    public PlayerCheckpointManager checkpoint;
    public HealthBarScript_FC healthBar;

    public GameObject character;
    public Transform position;

    [Header("Screen Shake when taking damages")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    [Header("Animation")]
    public Animator anim;
    public Animator deathtransition;
    public int maxfear;
    public int fear;
    public float noDamageTime;
    float coroutineCount;

    public AnimationCurve knockCurve;

    [System.NonSerialized] public bool isDead;
    [System.NonSerialized] public bool deadTP;
    public bool knockbacked;
    public bool noDamage;

    [Header("controler Vibration")]
    public float vibrationLeft;
    public float vibrationRight;
    PlayerIndex playerIndex;

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

        if (Input.GetButtonDown("Restart"))
        {
            fear = 0;
        }
    }
    void FixedUpdate()
    {
        DeathCheck();
    }

    public bool DeathCheck()
    {
        if ((fear <= 0 || fear == 0) && !isDead)
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
        isDead = true;
        FindObjectOfType<AudioManager>().Play("DeathSound");
        deathtransition.SetTrigger("StartFade");
        yield return new WaitForSeconds(2);
        deathtransition.SetTrigger("EndFade");
        deadTP = true;
        yield return new WaitForSeconds(1);
        deathtransition.ResetTrigger("EndFade");
        deathtransition.ResetTrigger("StartFade");
    }
    
    public void DealDamage(int damageValue)
    {
        if (!noDamage)
        {
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
            StartCoroutine(startVibration());
            fear -= damageValue;
            anim.SetBool("is Hurted", true);
            FindObjectOfType<AudioManager>().Play("Hurt");
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
        if(!noDamage)
        {
            controler.velocityMultiplicator = 0.2f;
            knockbacked = true;
            StartCoroutine(KnockBackCooldown(power, goingLeft));
        }        
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
    IEnumerator startVibration()
    {
        GamePad.SetVibration(playerIndex, vibrationLeft, vibrationRight);
        yield return new WaitForSeconds(0.5f);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }
}