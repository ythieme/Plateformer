using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpointManager : MonoBehaviour
{
    GameObject character;
    public FearScript_FC fear;
    public Controler_YT controler;

    public Vector3 lastCheckpoint;
    
    void Start()
    {        
        character = GameObject.FindGameObjectWithTag("Player");
        controler = character.GetComponent<Controler_YT>();
        lastCheckpoint = transform.position;
        fear = GameObject.Find("FearObject").GetComponent<FearScript_FC>();
    }
    
    void Update()
    {
        if (fear.fear <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        controler.enabled = false;
        character.transform.Translate(new Vector3(lastCheckpoint.x - character.transform.position.x, lastCheckpoint.y - character.transform.position.y));
        controler.enabled = true;
    }
}
