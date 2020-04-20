using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D thisBoxCollider;
    public BoxCollider2D parentBoxCollider;
    
    public float onPlatformTime;
    public float cooldown;

    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        thisBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlatformFall());
        }
    }

    IEnumerator PlatformFall()
    {        
        yield return new WaitForSeconds(onPlatformTime);
        spriteRenderer.enabled = false;
        parentBoxCollider.enabled = false;
        thisBoxCollider.enabled = false;
        Debug.Log("Ajouter animation disparition");
        StartCoroutine(PlatformRespawn());
    }

    IEnumerator PlatformRespawn()
    {
        yield return new WaitForSeconds(cooldown);
        spriteRenderer.enabled = true;
        parentBoxCollider.enabled = true;
        thisBoxCollider.enabled = true;
        Debug.Log("Ajouter animation réapparition");
    }
}
