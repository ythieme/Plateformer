using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("EnemieScore composents")]    
    public int touchedMalus;

    [System.NonSerialized] public int enemieScore;
    [System.NonSerialized] public int touchedNbr;

    [Header("Enemies Score")]
    public int sbireScore;
    public int flyingScore;
    public int mrPontScore;
    public int huggerScore;

    int sbireDetectedNbr;
    int flyingDetectedNbr;
    int mrPontDetectedNbr;
    int huggerDetectedNbr;
    bool stop;

    [Header("Time Score Composents")]
    public float moyCompTime;    
    public int scoreMultiplier;

    [System.NonSerialized] public int playerDeathNbr;
    [System.NonSerialized] public float sectionTimeScore;
    [System.NonSerialized] public int totalTimeScore;
    [System.NonSerialized] public float playerCompTime;
    [System.NonSerialized] public float totalTime;

    [Header("Others")]
    public BoxCollider2D boxCollider;
    public float detectorExtent;    
    public LayerMask enemy;
    
    [System.NonSerialized] public int totalScore;
    [System.NonSerialized] public int actualSection;

    void Start()
    {
        sbireDetectedNbr = 0;
        flyingDetectedNbr = 0;
        mrPontDetectedNbr = 0;
        huggerDetectedNbr = 0;
        actualSection = 0;
        playerDeathNbr = 0;
    }
    
    public RaycastHit2D ScoreDetector()
    {
        RaycastHit2D raycast = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + boxCollider.bounds.extents.y + detectorExtent),
            new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y - detectorExtent), enemy);

        return raycast;
    }

    void Update()
    {
        EnemyDetectionNbrDistribution();
        playerCompTime += Time.deltaTime;
    }

    /* Pour le Débug
    private void FixedUpdate()
    {
        Debug.Log("Temps de section : " + playerCompTime);
        Debug.Log("Section N°" + actualSection);
        Debug.Log("Temps total : " + totalTime);
    }
    */

    public void EnemyDetectionNbrDistribution()
    {
        if (ScoreDetector() && !stop)
        {
            if (ScoreDetector().collider.CompareTag("Sbire"))
            {
                sbireDetectedNbr = OneEnemyScore(sbireDetectedNbr);
            }
            else if (ScoreDetector().collider.CompareTag("Flying"))
            {
                flyingDetectedNbr = OneEnemyScore(flyingDetectedNbr);
            }
            else if (ScoreDetector().collider.CompareTag("Hugger"))
            {
                huggerDetectedNbr = OneEnemyScore(huggerDetectedNbr);
            }
            else if (ScoreDetector().collider.CompareTag("MrPont"))
            {
                mrPontDetectedNbr = OneEnemyScore(mrPontDetectedNbr);
            }
        }
    }

    int OneEnemyScore(int detectedNbr)
    {
        detectedNbr++;
        StartCoroutine(EnemyDetected());
        return detectedNbr;
    }
    IEnumerator EnemyDetected()
    {        
        stop = true;
        yield return new WaitForSeconds(1f);
        stop = false;
    }
    public int EnemyScoreCalculation()
    {
        enemieScore = ((sbireDetectedNbr * sbireScore) + (mrPontDetectedNbr * mrPontScore) + (flyingDetectedNbr * flyingScore) + (huggerDetectedNbr * huggerScore))
            - touchedNbr * touchedMalus;

        return enemieScore;
    }

    public int SectionTimeScore(float moyCompTime, float playerCompTime)
    {
        sectionTimeScore = (moyCompTime / playerCompTime);

        return Mathf.RoundToInt(sectionTimeScore);
    }

    public int FinalTimeScore()
    {
        totalTimeScore = totalTimeScore / (1 + playerDeathNbr) * scoreMultiplier;
        return totalTimeScore;
    }

    public int TotalScore()
    {
        totalScore = enemieScore + totalTimeScore;

        return totalScore;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + boxCollider.bounds.extents.y + detectorExtent),
            new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y - detectorExtent));
    }
}
