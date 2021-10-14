using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualSpeaker : MonoBehaviour
{
    private FMOD.Studio.EventInstance arrival;
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Arrival");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayArrival()
    {
       
        FMODUnity.RuntimeManager.PlayOneShot("event:/Arrival");
        //print("speakers");
    }
}
