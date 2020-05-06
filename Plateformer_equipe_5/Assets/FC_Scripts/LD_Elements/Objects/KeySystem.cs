using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySystem : MonoBehaviour
{
    public FearScript_FC fear;

    public SpriteRenderer sp;

    public bool keyCatched;
    public bool restartNeeded;

    private void Start()
    {
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(fear.isDead == true)
        {
            restartNeeded = true;
        }
        Restart();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyCatched = true;
            sp.enabled = false;
        }
    }
    void Restart()
    {
        if (restartNeeded)
        {
            keyCatched = false;
            sp.enabled = true;
            restartNeeded = false;
        }
    }
}
