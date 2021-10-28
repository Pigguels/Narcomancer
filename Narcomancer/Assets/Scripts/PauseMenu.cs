using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public Animator pauseMenuAnim;

    public void OnClickResume()
    {
        //Resume code here
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


}
