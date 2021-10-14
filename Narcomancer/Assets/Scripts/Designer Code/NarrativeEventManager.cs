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

    private float timer;
    private bool timerenabled;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
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
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            WaveMaster.GetComponent<WaveManager>().Wave2();
        }
    }
    public void StoryWave3()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave3Audio();
        timer = 14f;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            WaveMaster.GetComponent<WaveManager>().Wave3();
        }
    }
    public void StoryWave4()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave4Audio();
        timer = 15f;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            WaveMaster.GetComponent<WaveManager>().Wave4();
        }
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
