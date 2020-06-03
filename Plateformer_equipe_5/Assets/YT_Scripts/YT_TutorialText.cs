using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YT_TutorialText : MonoBehaviour
{
    [SerializeField] Animator image;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            image.SetBool("Visible", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            image.SetBool("Visible", false);
        }
    }
}
