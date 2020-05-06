using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public KeySystem key;
    public FearScript_FC fear;
    public GameObject door;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && key.keyCatched)
        {
            door.SetActive(false);
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
