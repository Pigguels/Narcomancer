using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Animator pauseMenuAnim;

    public void OnClickResume()
    {
        pauseMenuAnim.SetBool("Paused", false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        PlayerController.paused = false;
        Debug.Log("Game Resumed");
    }

    public void OnClickOptions()
    {
        //Options code here
        Debug.Log("Options");
    }

    public void OnClickQuit()
    {
        //Close Game
        Application.Quit();
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
