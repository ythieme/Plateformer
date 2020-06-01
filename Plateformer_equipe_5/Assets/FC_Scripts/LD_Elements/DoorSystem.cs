using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public KeySystem key;
    public FearScript_FC fear;
    public GameObject door;
    public Animator anim;

    private bool soundPlay;
    public bool doorOpen;
    public bool restartNeeded;

    private void Start()
    {
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
    }

    private void Update()
    {
        if (fear.isDead == true)
        {
            restartNeeded = true;
            anim.SetBool("is Opened", false);
        }
        Restart();
    }

    private void SoundPlay()
    {
        if (key.keyCatched && soundPlay == true)
        {
            FindObjectOfType<AudioManager>().Play("OpenDoor");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && key.keyCatched && !doorOpen)
        {
            soundPlay = true;
            anim.SetTrigger("Open");
            anim.SetBool("is Opened", true);
            SoundPlay();
            door.SetActive(false);
            doorOpen = true;
        }
    }
    void Restart()
    {
        if (restartNeeded)
        {
            door.SetActive(true);
            restartNeeded = false;
            doorOpen = false;
        }
    }
}
