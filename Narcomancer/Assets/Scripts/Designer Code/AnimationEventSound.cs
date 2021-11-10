using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSound : MonoBehaviour
{
    public enum SoundToBePlayed { Shotgun,Lightning,GruntDeath,RatDeath,EnfDeath}
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

            case SoundToBePlayed.GruntDeath:
                FMODUnity.RuntimeManager.PlayOneShot("event:/3D Dialogue/Death SFXs Voiceover/Baz's Death sounds", transform.position);
                print("gruntdeath");
                break;

        }
    }
}
