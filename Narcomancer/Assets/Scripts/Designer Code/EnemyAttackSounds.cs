using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSounds : MonoBehaviour
{
    public enum SoundToBePlayed {Smgshoot, Pistolshoot, EnfShoot, Magnumshoot, grenadelaunch, }
    public SoundToBePlayed sound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GunSound()
    {
        print("playsound");
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Shotgun SFX/Shotgun Blasts", transform.position);
        switch (sound)
        {
            case SoundToBePlayed.Smgshoot:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Enemy Weopons/Mini Uzi", transform.position);
                break;

            case SoundToBePlayed.Pistolshoot:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Enemy Weopons/Beretta", transform.position);
                //print("gruntdeath");
                break;

            case SoundToBePlayed.EnfShoot:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Shotgun SFX/Shotgun Blasts", transform.position);
                //print("gruntdeath");
                break;

            case SoundToBePlayed.Magnumshoot:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Enemy Weopons/Colt", transform.position);
                //print("gruntdeath");
                break;

            case SoundToBePlayed.grenadelaunch:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Grenade SFX/Grenade Launch", transform.position);
               // print("gruntdeath");
                break;
        }
    }
}
