using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class YT_ScoreMenu : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimationCurve Ralentissement;
    public GameObject firstScoreButton;
    private float temps;
    private float timer;
    private bool doSlowDown;
    public TMP_Text time;
    public TMP_Text timeScore;
    public TMP_Text ennemyScore;
    public TMP_Text numberDeath;
    public TMP_Text totalScore;
    int totalTimeScore;
    string stringTotalTimeScore;
    int totalEnnemyScore;
    string stringTotalEnnemyScore;
    int numberOfDeath;
    string stringNumberOfDeath;
    int globalscore;
    string stringTotalScore;

    [SerializeField] Animator scorePanelAnimation;
    [SerializeField] GameObject panel;

    
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstScoreButton);
    }

    private void Update()
    {
        if (doSlowDown && temps < 0.7)
        {
            StartCoroutine(SlowDown());
        }
        else if (doSlowDown && (temps == 0.7 || temps > 0.7))
        {
            doSlowDown = false;
            panel.SetActive(true);
            TotalTimeScore();
            EnnemyScore();
            NumberofDeath();
            GlobalScore();
            TimerTotal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            temps = 0f;
            timer = scoreManager.timer;
            scoreManager.EnemyScoreCalculation();
            scoreManager.FinalTimeScore();
            scoreManager.TotalScore();
            scorePanelAnimation.SetBool("Visible", true);
            doSlowDown = true;
        }
    }

    IEnumerator SlowDown()
    {
        temps += Time.deltaTime;
        Time.timeScale = Ralentissement.Evaluate(temps);
        yield return 0.1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    private void TimerTotal()
    {
        time.text = timer.ToString();
    }

    private void TotalTimeScore()
    {
        totalTimeScore = scoreManager.totalTimeScore;
        stringTotalTimeScore = totalTimeScore.ToString();
        timeScore.text = stringTotalTimeScore;
    }

    private void EnnemyScore()
    {
        totalEnnemyScore = scoreManager.enemieScore;
        stringTotalEnnemyScore = totalEnnemyScore.ToString();
        ennemyScore.text = stringTotalEnnemyScore;
    }

    private void NumberofDeath()
    {
        numberOfDeath = scoreManager.playerDeathNbr;
        stringNumberOfDeath = numberOfDeath.ToString();
        numberDeath.text = stringNumberOfDeath;
    }

    private void GlobalScore()
    {
        globalscore = scoreManager.totalScore;
        stringTotalScore = globalscore.ToString();
        totalScore.text = stringTotalScore;
    }

}

