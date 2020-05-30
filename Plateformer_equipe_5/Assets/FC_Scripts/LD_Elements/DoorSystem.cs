using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public KeySystem key;
    public FearScript_FC fear;
    public GameObject door;
    public GameObject doorTrigger;

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
        }
        Restart();
    }

    private bool SoundPlay()
    {
        if (key.keyCatched && soundPlay == true)
        {
            FindObjectOfType<AudioManager>().Play("OpenDoor");
        }
       return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && key.keyCatched)
        {
            soundPlay = true;
            SoundPlay();
            door.SetActive(false);
            doorTrigger.SetActive(false);
        }
    }
    void Restart()
    {
        if (restartNeeded)
        {
            door.SetActive(true);
            restartNeeded = false;
        }
    }
}
