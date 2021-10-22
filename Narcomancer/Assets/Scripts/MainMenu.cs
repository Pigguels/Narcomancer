using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    int sceneToLoad;
    public Animator optionsAnim;
    bool settingsOpen = false;

    float deltaTimer;
    float deltaTimer1;

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
}
