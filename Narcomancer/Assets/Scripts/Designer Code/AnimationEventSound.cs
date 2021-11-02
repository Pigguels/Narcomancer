using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSound : MonoBehaviour
{
    public enum SoundToBePlayed { Shotgun,Lightning}
    public SoundToBePlayed sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        print("playsound");
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Shotgun SFX/Shotgun Blasts", transform.position);
        switch (sound)
        {
            case SoundToBePlayed.Shotgun:
                 FMODUnity.RuntimeManager.PlayOneShot("event:/Shotgun SFX/Shotgun Blasts", transform.position);
                 break;

                
        }
    }
}
