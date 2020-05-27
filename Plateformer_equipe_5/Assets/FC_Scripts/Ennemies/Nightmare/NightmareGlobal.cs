using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NightmareGlobal : MonoBehaviour
{
    public PathCreator pathCreator;
    public FearScript_FC fear;
    Vector2 startPosition;

    [Header("Speed")]
    public float speed;
    float distanceTravelled;

    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play("SpawnNighmare");
    }
    private void Start()
    {
        fear = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FearScript_FC>();
        startPosition = transform.position;
    }

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;

        if (fear.fear == 0 || fear.fear < 0)
        {
            distanceTravelled = 0;
            transform.Translate(new Vector3(startPosition.x - transform.position.x, startPosition.y - transform.position.y));
            this.gameObject.SetActive(false);
        }
        else
        {
             transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        }
    }
}
