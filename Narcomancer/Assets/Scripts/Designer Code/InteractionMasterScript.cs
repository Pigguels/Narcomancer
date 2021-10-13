using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMasterScript : MonoBehaviour
{
    [System.Serializable]
    public enum EventTriggerName {Door, Money, Powerbox, Drugs, Phone,};
    public EventTriggerName eventName;
    public GameObject switchObject;
    public bool runtest;

    void Start()
    {
        if (runtest == true)
        {
            Interact();
        }

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void Interact()
    {
        switch (eventName)
        {
            //REPLACES FAKE DOOR WITH REAL DOOR AND THEN ANIMATES IT
            case EventTriggerName.Door:
                print("Player kicks open door");
                switchObject.SetActive(true);
                switchObject.SendMessage("Run");
                gameObject.SetActive(false);   
                break;
            ////"SHOULD" REPLACE MONEY WITH INERT VERSION WHILE CALLING THE BADE ENDING UI - NEEDS UI INFO
            case EventTriggerName.Money:
                print("Player takes the money");
                switchObject.SetActive(true);
                switchObject.SendMessage("Run");
                gameObject.SetActive(false);
                break;
            //REPLACES POWERBOX WITH OFF VERSION, OFF VERSION HAS SCRIPT FOR BLACKOUT SEQUENCE AND TO RESET AFTER X SECONDS
            case EventTriggerName.Powerbox:
                print("Player turns off the power");
                switchObject.SetActive(true);
                gameObject.SetActive(false);
                break;
            //REMOVES DRUGS, PLACES PARTICLE EFFECT WITH SCRIPT TO TRIGGER NARRATIVE
            case EventTriggerName.Drugs:
                print("Player destorys the drugs");
                switchObject.SetActive(true);
                gameObject.SetActive(false);
                break;
            //SWITCHES PHONE OUT FOR INERT VERSION AND RUNS NARRATIVE SCRIPT
            case EventTriggerName.Phone:
                print("answered the phone");
                switchObject.SetActive(true);
                gameObject.SetActive(false);

                break;


        }
    }

}
