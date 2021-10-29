using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    int sceneToLoad;
    public Animator optionsAnim;
    bool settingsOpen = false;

    float deltaTimer;
    float deltaTimer1;

    public TMP_Dropdown fullscreenDropdown;
    public TMP_Dropdown resolutionDropdown;

    private void Update()
    {
        if (deltaTimer1 <= deltaTimer)
        {
            deltaTimer1 += Time.deltaTime;
        }

        

    }

    public void OnClickPlay()
    {
        Debug.Log("Load Scene" + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnClickOptions()
    {
        Debug.Log("Opening Options Menu");

        if (deltaTimer1 > deltaTimer)
        {
            if (settingsOpen == false)
            {
                optionsAnim.SetBool("SettingsOpen", true);
                settingsOpen = true;
            }
            else
            {
                optionsAnim.SetBool("SettingsOpen", false);
                settingsOpen = false;
            }
        }
        deltaTimer1 = 0f;


    }

    public void OnClickCredits()
    {
        Debug.Log("Opening Credits");
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }


    public void FullscreenChange()
    {
        if (fullscreenDropdown.value == 0)
        {
            Screen.fullScreen = true;
        }

        if (fullscreenDropdown.value == 1)
        {
            Screen.fullScreen = false;
        }
    }


    public void ResolutionChange()
    {
        if (resolutionDropdown.value == 0)
        {
            Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
        }
        if (resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        }
        if (resolutionDropdown.value == 2)
        {
            Screen.SetResolution(1280, 720, Screen.fullScreenMode);
        }
        if (resolutionDropdown.value == 3)
        {
            Screen.SetResolution(2560, 1600, Screen.fullScreenMode);
        }
        if (resolutionDropdown.value == 4)
        {
            Screen.SetResolution(1920, 1200, Screen.fullScreenMode);
        }
        if (resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1280, 800, Screen.fullScreenMode);
        }
    }
}
