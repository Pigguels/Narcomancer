using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour
{
    public GameObject speaker1;
    public GameObject speaker2;
    public GameObject speaker3;

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
        speaker1.GetComponent<IndividualSpeaker>().PlayArrival();
       //speaker2.GetComponent<IndividualSpeaker>().PlayArrival();
       //speaker3.GetComponent<IndividualSpeaker>().PlayArrival();
        print("parent");
    }

    public void Wave2Audio()
    {
        speaker1.GetComponent<IndividualSpeaker>().PlayWave2();
        speaker2.GetComponent<IndividualSpeaker>().PlayWave2();
        speaker3.GetComponent<IndividualSpeaker>().PlayWave2();
    }

    public void Wave3Audio()
    {
        speaker1.GetComponent<IndividualSpeaker>().PlayWave3();
        speaker2.GetComponent<IndividualSpeaker>().PlayWave3();
        speaker3.GetComponent<IndividualSpeaker>().PlayWave3();
    }

    public void Wave4Audio()
    {
        speaker1.GetComponent<IndividualSpeaker>().PlayWave4();
        speaker2.GetComponent<IndividualSpeaker>().PlayWave4();
        speaker3.GetComponent<IndividualSpeaker>().PlayWave4();
    }

}
