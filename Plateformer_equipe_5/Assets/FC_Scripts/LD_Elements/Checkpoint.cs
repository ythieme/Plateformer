using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointRespawnObject;
    public BoxCollider2D boxCollider;
    public Vector3 checkpointPosition;

    public PlayerCheckpointManager cpManager;

    void Start()
    {
        checkpointPosition = checkpointRespawnObject.transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cpManager.lastCheckpoint = checkpointPosition;
        }
    }        

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position,boxCollider.bounds.size);
        Gizmos.DrawLine(transform.position,checkpointRespawnObject.transform.position);
    }
}
