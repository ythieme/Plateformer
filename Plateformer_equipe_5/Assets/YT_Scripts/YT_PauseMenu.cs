using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class YT_PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject controler;
    public GameObject pauseFirstButton;
    public GameObject optionsClosedButton;
    public GameObject pauseMenuiUi;
    public GameObject optionMenuUI;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject optionsFirstButton;
    public GameObject tpMenu;


    public void PauseMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
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
        tpMenu.SetActive(false);
        Time.timeScale = 1f;
        controler.GetComponent<Controler_YT>().enabled = true;
        GameIsPaused = false;
    }
    public void SounndResume()
    {
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }

    void Pause()
    {
        pauseMenuiUi.SetActive(true);
        controler.GetComponent<Controler_YT>().enabled = false;
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        GameIsPaused = true;

    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("quitting");
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        Application.Quit();
    }
}
