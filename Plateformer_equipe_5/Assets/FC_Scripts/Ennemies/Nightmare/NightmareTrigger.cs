using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareTrigger : MonoBehaviour
{
    public GameObject nightmare;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) nightmare.SetActive(true);        
    }
}
