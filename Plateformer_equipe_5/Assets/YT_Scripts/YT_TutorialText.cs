using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YT_TutorialText : MonoBehaviour
{
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator1.SetBool("Visible", true);
            animator2.SetBool("Visible", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator1.SetBool("Visible", false);
            animator2.SetBool("Visible", false);
        }
    }
}
