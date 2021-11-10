using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public PlayerController player;
    public GameObject settingMenu;
    bool optionsOpen = false;

    public void OnClickResume()
    {
        player.OnPauseUI();
        Debug.Log("Game Resumed");
    }

    public void OnClickOptions()
    {
        if (!optionsOpen)
        {
            settingMenu.SetActive(true);
            Debug.Log("Options");
            optionsOpen = true;
        } else
        {
            settingMenu.SetActive(false);
            Debug.Log("Options");
            optionsOpen = false;
        }

    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(1);
    }


}
