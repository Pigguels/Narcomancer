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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Arrival", transform.position);
        print("speakers");
    }

    public void PlayWave2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave 2 Start)", transform.position);
    }
    public void PlayWave3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave 3 Start)", transform.position);
    }
    public void PlayWave4()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave 4 Start)", transform.position);
    }



}
