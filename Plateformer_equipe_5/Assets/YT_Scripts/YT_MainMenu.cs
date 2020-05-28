﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class YT_MainMenu : MonoBehaviour
{
    public Controler_YT controler;
    public GameObject optionMenu;
    public GameObject pauseMenu;
    public GameObject pauseFirstButton;
    public GameObject optionsClosedButton;
    public GameObject optionsFirstButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
   
    public void PauseMenu()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);

    }
    public void PlayGame() 
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        Application.Quit();
    }
}
