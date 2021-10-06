using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public GameObject UiController;
    // Start is called before the first frame update
    void Start()
    {
        UiController.GetComponent<BadEndingScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run()
    {
        UiController.BadEnding();
    }
}
