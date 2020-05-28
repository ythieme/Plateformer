using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class NightmareTrigger : MonoBehaviour
{
    [Header("screen Shake Nighmare")]
    public float magnitudeN;
    public float roughnessN;
    public float fadeInTimeN;
    public float fadeOutTimeN;

    public GameObject nightmare;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))        
        {
            CameraShaker.Instance.ShakeOnce(magnitudeN, roughnessN, fadeInTimeN, fadeOutTimeN);
            nightmare.SetActive(true);
            nightmare.GetComponent<EnemyDamageProcess>().inCooldown = false;
        }            
    }
}
