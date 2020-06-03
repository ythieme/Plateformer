using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;

public class NightmareTrigger : MonoBehaviour
{
    [Header("screen Shake Nighmare")]
    public float magnitudeN;
    public float roughnessN;
    public float fadeInTimeN;
    public float fadeOutTimeN;
    
    [Header("controler Vibration")]
    public float vibrationLeft;
    public float vibrationRight;
    PlayerIndex playerIndex;

    public GameObject nightmare;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))        
        {
            CameraShaker.Instance.ShakeOnce(magnitudeN, roughnessN, fadeInTimeN, fadeOutTimeN);
            StartCoroutine(startVibration());
            nightmare.SetActive(true);
            nightmare.GetComponentInChildren<EnemyDamageProcess>().inCooldown = false;
        }            
    }

    IEnumerator startVibration()
    {
        GamePad.SetVibration(playerIndex, vibrationLeft, vibrationRight);
        yield return new WaitForSeconds(1f);
        GamePad.SetVibration(playerIndex,0f,0f);
    }
}
