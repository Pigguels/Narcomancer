﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeTriggerBox : MonoBehaviour
{
    public enum TriggerBoxName {Intro, Office, Bossfight, Monologue, Vip }
    public TriggerBoxName triggerName;
    public GameObject NarrativeEventController;
    //public bool istriggered;
 
    // Start is called before the first frame update
    void Start()
    {
       // istriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") //!istriggered 
        {
            
            switch (triggerName)
            {
                case TriggerBoxName.Intro:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryIntroduction();
                    gameObject.SetActive(false);
                    print("entryway trigger");
                    break;
                case TriggerBoxName.Office:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryPhoneCall();
                    break;
                case TriggerBoxName.Bossfight:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryBossFight();
                    print("donde esta boss fight?");
                    break;
                case TriggerBoxName.Monologue:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryMonologue();
                    break;
                case TriggerBoxName.Vip:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().Gentlemen();
                    break;
            }
            //print("triggered");
            gameObject.SetActive(false);
        }
       
    }

 //   private void OnTriggerExit(Collider other)
   // {
     //  if (other.tag == "Player" && istriggered){ gameObject.SetActive(false); }
    //}
}
