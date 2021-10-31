using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject Wave1Controller;
    public GameObject Wave2Controller;
    public GameObject Wave3Controller;
    public GameObject Wave4Controller;
    public GameObject PostEnforcerController;
    public GameObject AlarmRatsController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Wave1()
    {
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
        Wave4Controller.SetActive(true);
    }

    public void PostEnforcer()
    {
        PostEnforcerController.SetActive(true);
    }

    public void AlarmRats()
    {
        AlarmRatsController.SetActive(true);
    }

}
