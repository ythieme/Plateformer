using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpointManager : MonoBehaviour
{
    GameObject character;
    public FearScript_FC fear;
    public Controler_YT controler;
    public ScoreManager score;
    public Vector3 lastCheckpoint;
    
    void Start()
    {        
        character = GameObject.FindGameObjectWithTag("Player");
        controler = character.GetComponent<Controler_YT>();
        lastCheckpoint = transform.position;
        fear = GameObject.Find("FearObject").GetComponent<FearScript_FC>();
        score = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ScoreManager>();
    }
    
    void Update()
    {
        if (fear.fear <= 0 && fear.deadTP)
        {
            Death();
            fear.deadTP = false;
        }
    }            

    public void Death()
    {
        controler.enabled = false;
        character.transform.Translate(new Vector3(lastCheckpoint.x - character.transform.position.x, lastCheckpoint.y - character.transform.position.y));
        controler.enabled = true;
        fear.fear = fear.maxfear;
        score.playerDeathNbr++;
    }
}
