using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtoInteract : MonoBehaviour
{
    public GameObject UiElement;
    public bool interactedwith = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (interactedwith == false))
        {
            UiElement.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") && (interactedwith == false))
        {
            print("i have left the zone");
            UiElement.SetActive(false);
        }
    }

    public void HidePrompt()
    {
        UiElement.SetActive(false);
        interactedwith = true;
    }
}
