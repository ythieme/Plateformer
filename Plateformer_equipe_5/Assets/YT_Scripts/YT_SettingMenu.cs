﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YT_SettingMenu : MonoBehaviour
{
    public FearScript_FC fear;
    public GameObject optionsFirstButton;
    public GameObject optionMenu;
    public GameObject pauseFirstButton;
    public GameObject pauseMenu;
    public AudioMixer audiomixer;
    public Dropdown resolutionDropDown;

    Resolution[] resolution;

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    public void SetVolume (float volume)
    {
        audiomixer.SetFloat("YT_Volume", volume);
    }

    public void SetNotDamages(bool noDamages)
    {
        fear.noDamage = noDamages == false;
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        resolution = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentRosolutionIndex = 0;
        for (int i = 0; i < resolution.Length; i++)
        {
            string option = resolution[i].width + "x" + resolution[i].height;
            options.Add(option);

            if (resolution[i].width == Screen.currentResolution.width && resolution[i].height == Screen.currentResolution.height)
            {
                currentRosolutionIndex = i;

            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentRosolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolutions = resolution[resolutionIndex];
        Screen.SetResolution(resolutions.width, resolutions.height, Screen.fullScreen);
    }
}
