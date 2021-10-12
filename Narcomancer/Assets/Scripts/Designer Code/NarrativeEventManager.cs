﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{   
    [Header ("Controller Objects")]
    public GameObject waveController;

    [Header("Dialogue and Animation Objects")]
    public GameObject narcomancer;
    public GameObject speakerParent;
    public GameObject phone;
    public GameObject henchman;
    public GameObject officeWindow;

    [Header("Trigger Objects")]
    public GameObject introductionTrigger;
    public GameObject officeArrivalTrigger;
    public GameObject officeEscapeTrigger;
    public GameObject monologueTrigger;


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
        // FMODUnity.RuntimeManager.PlayOneShot(narcoIntro);
        //Wavecontroller.wave1
        speakerParent.GetComponent<DialogueSpeaker>().ArrivalAudio();
        //introductionTrigger.SetActive(false);
        print("managaer");
    }
    public void StoryWave2()
    {
        //fmod play wave start A
        //Wavecontroller.wave2
    }
    public void StoryWave3()
    {
        //fmod play wave start B
        //Wavecontroller.wave3
    }
    public void StoryWave4()
    {
        //fmod play wave start c
        //Wavecontroller.wave4
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