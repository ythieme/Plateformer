using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    public FearScript_FC fear;

    SpriteRenderer spriteRenderer;
    BoxCollider2D thisBoxCollider;
    Animator anim;
    public BoxCollider2D parentBoxCollider;
    
    public float onPlatformTime;
    public float cooldown;

    bool isFalling;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        thisBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (fear.fear < 0 || fear.fear == 0)
        {
            spriteRenderer.enabled = true;
            parentBoxCollider.enabled = true;
            thisBoxCollider.enabled = true;
            isFalling = false;
            StopAllCoroutines();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            StartCoroutine(PlatformFall());
        }
    }

    IEnumerator PlatformFall()
    {        
        yield return new WaitForSeconds(onPlatformTime - 0.25f);
        anim.SetTrigger("Tombe");
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.enabled = false;
        parentBoxCollider.enabled = false;
        thisBoxCollider.enabled = false;
        StartCoroutine(PlatformRespawn());
    }
    

    IEnumerator PlatformRespawn()
    {
        yield return new WaitForSeconds(cooldown);
        spriteRenderer.enabled = true;
        parentBoxCollider.enabled = true;
        thisBoxCollider.enabled = true;
        isFalling = false;
    }
}
