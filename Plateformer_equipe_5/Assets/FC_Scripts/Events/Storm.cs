using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    public Animator anim;

    public float minTime;
    public float maxTime;
    public float cooldownTime;
    public float timeLastLightning;

    public bool inStorm;
    public bool inCooldown;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inStorm = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inStorm = false;
            timeLastLightning = 0;
        }
    }    
    public void Lightning()
    {
        if (!inCooldown)
        {
            inCooldown = true;
            RandomCooldown();
        }
    }

    void RandomCooldown()
    {
        cooldownTime = Random.Range(minTime,maxTime);
    }

    IEnumerator LightningAnim()
    {
        if (timeLastLightning == cooldownTime || timeLastLightning > cooldownTime)
        {
            //processus de "Flash" à intégrer ici
            timeLastLightning = 0;
            inCooldown = false;
            anim.SetTrigger("Thunder");
            yield return new WaitForSeconds(0.1f);
            FindObjectOfType<AudioManager>().Play("Thunder");
        }
    }

    void FixedUpdate()
    {        
        if (inStorm)
        {
            Lightning();
            timeLastLightning += Time.deltaTime;
            if (inCooldown)
            {
                StartCoroutine(LightningAnim());
            }
        }
    }
}