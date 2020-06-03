using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    public AudioSource epicness;

    public bool isEnd;
    public bool isPlaying;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isEnd && !isPlaying)
        {
            isPlaying = true;
            epicness.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isEnd)
        {
            epicness.Stop();
        }
    }
}
