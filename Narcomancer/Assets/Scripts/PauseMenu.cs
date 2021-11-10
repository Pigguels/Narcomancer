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

 //   public void OnClickOptions()
   // {
 //       if (!optionsOpen)
   //     {
     //       settingMenu.SetActive(true);
       //     Debug.Log("Options");
  //          optionsOpen = true;
   //     } else
    //    {
      //      settingMenu.SetActive(false);
        //    Debug.Log("Options");
          //  optionsOpen = false;
     //   }

    //}

    public void OnClickQuit()
    {
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
