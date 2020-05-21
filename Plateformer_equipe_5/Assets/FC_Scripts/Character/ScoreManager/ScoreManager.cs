using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("EnemieScore composents")]
    public int touchedNbr;
    public int touchedMalus;
    public int enemieScore;

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
    public float playerCompTime;
    public int scoreMultiplier;
    public int playerDeathNbr;
    public float sectionTimeScore;
    public int totalTimeScore;

    [Header("Others")]
    public BoxCollider2D boxCollider;
    public float detectorExtent;
    public int actualSection;
    public LayerMask enemy;
    public int totalScore;

    void Start()
    {
        actualSection = 0;
        playerDeathNbr = 0;
    }
    
    public RaycastHit2D ScoreDetector()
    {
        RaycastHit2D raycast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + boxCollider.bounds.extents.y + detectorExtent),
            Vector2.down, - (boxCollider.bounds.size.y + 2* detectorExtent), enemy);

        return raycast;
    }

    void Update()
    {
        EnemyDetectionNbrDistribution();
        playerCompTime += Time.deltaTime;        
    }

    public void EnemyDetectionNbrDistribution()
    {
        if (ScoreDetector() && !stop)
        {
            if (ScoreDetector().collider.CompareTag("Sbire"))
            {
                StartCoroutine(EnemyDetected(sbireDetectedNbr));
            }
            else if (ScoreDetector().collider.CompareTag("Flying"))
            {
                StartCoroutine(EnemyDetected(flyingDetectedNbr));
            }
            else if (ScoreDetector().collider.CompareTag("Hugger"))
            {
                StartCoroutine(EnemyDetected(huggerDetectedNbr));
            }
            else if (ScoreDetector().collider.CompareTag("MrPont"))
            {
                StartCoroutine(EnemyDetected(mrPontDetectedNbr));
            }
        }
    }
    IEnumerator EnemyDetected(int detectedNbr)
    {        
        stop = true;
        detectedNbr++;
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
