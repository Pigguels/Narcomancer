using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{   
    [Header ("Controller Objects")]
    public GameObject WaveMaster;

    [Header("Dialogue and Animation Objects")]
    public GameObject narcomancer;
    public GameObject speakerParent;
    public GameObject phone;
    public GameObject henchman;
    public GameObject officeWindow;

    [Header("Trigger Objects")]
    
    public GameObject officeArrivalTrigger;
    public GameObject officeEscapeTrigger;
    public GameObject monologueTrigger;

    public float timer;
    private bool timerenabled;

    public bool wave2;
    public bool wave3;
    public bool wave4;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (timerenabled)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            timerenabled = false;
        }

        if (!timerenabled && wave2 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave2();
            wave2 = false;
        }
        if (!timerenabled && wave3 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave3();
            wave3 = false;
        }
        if (!timerenabled && wave4 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave4();
            wave4 = false;
        }

    }

    public void StoryIntroduction()
    {
        
        //Wavecontroller.wave1
        speakerParent.GetComponent<DialogueSpeaker>().ArrivalAudio();
     
        
    }
    public void StoryWave2()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave2Audio();
        timer = 12.7f;
        timerenabled = true;
        wave2 = true;       
    }

    public void StoryWave3()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave3Audio();
        timer = 14f;
        timerenabled = true;
        wave3 = true;
    }
    public void StoryWave4()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave4Audio();
        timer = 15f;
        timerenabled = true;
        wave4 = true;
    }

    public void StoryVipRoom()
    {
        //fmod play VIPROOM
        //anim narco shoot henchman
        //anim henchman dies
        //wavecontroller.postenforcer 
        //officeArrivalTrigger.SetActive
    }
    public void StoryPhoneCall()
    {
        
    }
    public void StoryOnThePhone()
    {
        //fmod stop phone
        //fmod pick up phone
        //fmod arrive at office
    }
    public void StoryDestroyTheDrugs()
    {
        //THIS HAS A TRIGGER SET UP
        //fmod play sounds "drugreaction"
        // cage.animationsettrigger
        //officeEscapeTrigger.setactibe(true)
        //waitforsecond(animationtime)   
      //narcomanceranim.setrigger.shoot
       // WaitForSeconds(//animationtime);
        //Officeglass.shootingouttheglass()
    }
    public void StoryBossFight()
    {
        //fmod fightstart taunt
        //wavcontroller.bossWave1
    }

    public void NarcomancerDeated()
    {

        //narcomanceranim.settrigger dying
        //fmod dying noises (dont have any)
        monologueTrigger.SetActive(true);


    }
    public void StoryMonologue()
    {
        //fmod monologue
        //narcomanim.settrigger monolgoue
    }
    

   
}
