using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualSpeaker : MonoBehaviour
{
    private FMOD.Studio.EventInstance arrival;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayArrival()
    {  
        FMODUnity.RuntimeManager.PlayOneShot("event:/Arrival");
        print("speakers");
    }

    public void PlayWave2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Barks (1)");
    }
    public void PlayWave3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Barks (2)");
    }
    public void PlayWave4()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Barks (3)");
    }



}
