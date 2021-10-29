using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeTriggerBox : MonoBehaviour
{
    public enum TriggerBoxName {Intro, Office, Bossfight, Monologue }
    public TriggerBoxName triggerName;
    public GameObject NarrativeEventController;
    public bool istriggered;
 
    // Start is called before the first frame update
    void Start()
    {
        istriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !istriggered)
        {
            istriggered = true;
            switch (triggerName)
            {
                case TriggerBoxName.Intro:
                    NarrativeEventController.GetComponent<NarrativeEventManager>().StoryIntroduction();
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
            gameObject.SetActive(false);
        }
        istriggered = true;
    }

 //   private void OnTriggerExit(Collider other)
   // {
     //  if (other.tag == "Player" && istriggered){ gameObject.SetActive(false); }
    //}
}
