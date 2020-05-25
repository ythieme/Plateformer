using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointRespawnObject;
    public BoxCollider2D boxCollider;
    public ScoreManager score;
    public CheckpointNumber checkpointNumber;
    public Vector3 checkpointPosition;

    public PlayerCheckpointManager cpManager;

    void Start()
    {
        checkpointPosition = checkpointRespawnObject.transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        score = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ScoreManager>();
        checkpointNumber = GetComponent<CheckpointNumber>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cpManager.lastCheckpoint = checkpointPosition;
        }

        if (other.gameObject.CompareTag("Player") && score.actualSection == checkpointNumber.checkpointNumber - 1)
        {
            score.totalTimeScore += score.SectionTimeScore(score.moyCompTime,score.playerCompTime);
            score.moyCompTime = checkpointNumber.compTime;
            score.totalTime += score.playerCompTime;
            score.playerCompTime = 0;
            score.actualSection ++;
        }
    }        

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position,boxCollider.bounds.size);
        Gizmos.DrawLine(transform.position,checkpointRespawnObject.transform.position);
    }
}
