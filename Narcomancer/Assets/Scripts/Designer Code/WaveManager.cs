using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject Wave1Controller;
    public GameObject Wave1AI;
    public GameObject Wave2Controller;
    public GameObject Wave3Controller;
    public GameObject Wave4Controller;
    public GameObject Wave4AI;
    public GameObject PostEnforcerAi;
    public GameObject PostEnforcerSMGs;
    public GameObject AlarmRatsController;
    public GameObject Boss1Controller;
    public GameObject Boss2Controller;
    public GameObject Boss3Controller;
    // Start is called before the first frame update
    void Start()
    {
        Wave1Controller.SetActive(true);
        //Wave4Controller.SetActive(true);
        //Wave4Controller
        //PostEnforcerController.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Wave1()
    {
         Wave1AI.BroadcastMessage("SpawnActive");
        Wave1Controller.SetActive(true);
       
    }
    public void Wave2()
    {
        Wave2Controller.SetActive(true);
    }
    public void Wave3()
    {
        Wave3Controller.SetActive(true);
    }
    public void Wave4()
    {
        Wave4AI.BroadcastMessage("SpawnActive");
        Wave4Controller.SetActive(true);
    }
    public void PostEnforcer()
    {
        PostEnforcerSMGs.SetActive(true);
    }
    public void GentlemensClub()
    {
        PostEnforcerAi.BroadcastMessage("SpawnActive");
    }
    public void AlarmRatsStart()
    {
        AlarmRatsController.SetActive(true);
    }
    public void AlarmRatsStop()
    {
        AlarmRatsController.SetActive(false);
    }
    public void Boss1()
    {
        Boss1Controller.SetActive(true);
    }
    public void Boss2()
    {
        Boss2Controller.SetActive(true);
    }
    public void Boss3()
    {
        Boss3Controller.SetActive(true);
    }
}
