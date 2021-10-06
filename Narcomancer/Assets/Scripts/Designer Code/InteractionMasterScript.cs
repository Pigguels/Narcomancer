using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMasterScript : MonoBehaviour
{
    [System.Serializable]
    public enum EventTriggerName {Door, Money, Powerbox, Drugs};
    public EventTriggerName eventName;
    public GameObject switchObject;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void Interact()
    {
        switch (eventName)
        {
            case EventTriggerName.Door:
                print("Player kicks open door");
                gameObject.SendMessage("Run");
                break;
            case EventTriggerName.Money:
                print("Player takes the money");
                gameObject.SetActive(false);
                break;
            case EventTriggerName.Powerbox:
                print("Player turns off the power");
                switchObject.SetActive(true);
                gameObject.SetActive(false);
                break;
            case EventTriggerName.Drugs:
                print("Player destorys the drugs");
                switchObject.SetActive(true);
                gameObject.SetActive(false);
                break;


        }
    }

}
