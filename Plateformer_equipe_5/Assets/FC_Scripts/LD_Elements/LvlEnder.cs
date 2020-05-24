using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlEnder : MonoBehaviour
{
    public ScoreManager score;

    void Start()
    {
        score = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            score.totalTimeScore += score.SectionTimeScore(score.moyCompTime, score.playerCompTime);
            score.moyCompTime = 0;
            score.playerCompTime = 0;

            score.TotalScore();
            PlayerPrefs.SetInt("Score", score.totalScore);
            LvlEnderProcess();
        }
    }

    public void LvlEnderProcess()
    {
        //Faire jouer la "cinématique" de fin de niveau
        //puis changer de scène vers la scène de tableau des scores
    }
}
