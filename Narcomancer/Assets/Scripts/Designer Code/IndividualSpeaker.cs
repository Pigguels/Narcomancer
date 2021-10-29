using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualSpeaker : MonoBehaviour
{
    private FMOD.Studio.EventInstance arrival;
    // Start is called before the first frame update
    void Start()
    {
        arrival = FMODUnity.RuntimeManager.CreateInstance("event:/3D Dialogue/Arrival");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayArrival()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/2D Sounds/2D Arrival Dialogue", transform.position,);
        //print("speakers");

       
        arrival.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, 25f);
        arrival.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, 100f);
        arrival.start();
    }

    public void PlayWave2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Starts (1)", transform.position);
    }
    public void PlayWave3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Starts (2)", transform.position);
    }
    public void PlayWave4()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Wave Starts (4)", transform.position);
    }



}
