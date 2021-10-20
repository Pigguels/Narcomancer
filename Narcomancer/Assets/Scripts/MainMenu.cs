using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    int sceneToLoad;

    public void OnClickPlay()
    {
        Debug.Log("Load Scene" + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnClickOptions()
    {
        Debug.Log("Opening Options Menu");
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
