using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatTP : MonoBehaviour
{
    public GameObject player;

    public Transform[] tpZones = new Transform[0];

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TP(Transform position)
    {
        player.transform.Translate(position.position);
    }
}
