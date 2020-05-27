using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YT_SettingMenu : MonoBehaviour
{
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

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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
}
