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


}
