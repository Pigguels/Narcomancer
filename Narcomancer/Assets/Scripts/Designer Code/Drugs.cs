using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drugs : MonoBehaviour
{
    public GameObject NarrativeController;
    // Start is called before the first frame update
    void Start()
    {
        NarrativeController.GetComponent<NarrativeEventManager>().StoryDestroyTheDrugs();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
