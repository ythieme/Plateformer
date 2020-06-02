using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YT_SettingMenu : MonoBehaviour
{
    public FearScript_FC fear;
    public CheatTP tp;
    public GameObject optionsFirstButton;
    public GameObject optionMenu;
    public GameObject pauseFirstButton;
    public GameObject pauseMenu;
    public AudioMixer audiomixer;
    public Dropdown resolutionDropDown;
    public GameObject tpMenu;
    public GameObject tpMenuFirstButton;
    public GameObject tpbutton;

    Resolution[] resolution;

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    public void TPMenu()
    {
        optionMenu.SetActive(false);
        tpMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(tpMenuFirstButton);
    }
    public void BackButtonOptions()
    {
        tpMenu.SetActive(false);
        optionMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(tpbutton);
    }
    public void CP1()
    {
        tp.TP(tp.tpZones[0]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP2()
    {
        tp.TP(tp.tpZones[1]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP3()
    {
        tp.TP(tp.tpZones[2]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP4()
    {
        tp.TP(tp.tpZones[3]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP5()
    {
        tp.TP(tp.tpZones[4]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP6()
    {
        tp.TP(tp.tpZones[5]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP7()
    {
        tp.TP(tp.tpZones[6]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP8()
    {
        tp.TP(tp.tpZones[7]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP9()
    {
        tp.TP(tp.tpZones[8]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void CP10()
    {
        tp.TP(tp.tpZones[9]);
        FindObjectOfType<AudioManager>().Play("ClickInterface");
    }
    public void SetVolume (float volume)
    {
        audiomixer.SetFloat("YT_Volume", volume);
    }
    public void SetNotDamages(bool noDamages)
    {
        fear.noDamage = noDamages == false;
        fear.StopAllCoroutines();
        fear.anim.SetBool("is Hurted", false);
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
