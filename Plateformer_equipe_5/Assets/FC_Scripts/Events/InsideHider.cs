using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideHider : MonoBehaviour
{
    public Animator anim;

    public bool hider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hider)
        {
            anim.SetTrigger("Alight");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hider)
        {
            anim.SetTrigger("Hide");
        }
    }
}
