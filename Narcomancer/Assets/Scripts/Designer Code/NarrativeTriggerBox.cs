using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeTriggerBox : MonoBehaviour
{
    public enum TriggerBoxName {Intro, Office, Bossfight, Monologue }
    public TriggerBoxName triggerName;
    public GameObject NarrativeEventController;
    public bool isTriggered = false;
 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
            return;
            
        
        if (other.tag == "Player")
        {
            switch (triggerName)
            {
                case TriggerBoxName.Intro:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryIntroduction();
                    print("triggered");
                    gameObject.SetActive(false);
                    break;
                case TriggerBoxName.Office:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryPhoneCall();
                    break;
                case TriggerBoxName.Bossfight:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryBossFight();
                    break;
                case TriggerBoxName.Monologue:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryMonologue();
                    break;

            }
            print("triggered");
           // gameObject.SetActive(false);
        }
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
