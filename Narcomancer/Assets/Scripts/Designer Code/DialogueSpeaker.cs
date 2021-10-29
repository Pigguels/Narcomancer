using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour
{
    public GameObject Arrival;
    public GameObject Wave2;
    public GameObject Wave3;
    public GameObject Wave4;
    public GameObject WaveBark;
    public GameObject VipRoom1;
    public GameObject VipHenchman;
    public GameObject VipRoom2;
    public GameObject phonedialogue1;
    public GameObject phonedialogue2;
    public GameObject drugsdestroyed;

    // public GameObject ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrivalAudio()
    {
        Arrival.SetActive(true);
        print("why wont this turn on");
    }

    public void Wave2Audio()
    {
        Wave2.SetActive(true);
    }

    public void Wave3Audio()
    {
        Wave3.SetActive(true);
    }

    public void Wave4Audio()
    {
        Wave4.SetActive(true);
    }

    public void PhoneDialogue()
    {
        phonedialogue1.SetActive(true);
    }

    public void DestroyedDrugs()
    {
        drugsdestroyed.SetActive(true);
    }
}
