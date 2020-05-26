using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YT_PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject controler;

    public GameObject pauseMenuiUi;
    public GameObject optionMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuiUi.SetActive(false);
        optionMenuUI.SetActive(false);
        Time.timeScale = 1f;
        controler.GetComponent<Controler_YT>().enabled = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuiUi.SetActive(true);
        controler.GetComponent<Controler_YT>().enabled = false;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
}
