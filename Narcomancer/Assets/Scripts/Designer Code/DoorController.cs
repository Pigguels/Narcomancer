using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public GameObject StairwayDoorTR1;
    public GameObject StairwayDoorTR2;
    public GameObject StairwayDoorBR1;
    public GameObject StairwayDoorBR2;
    public GameObject StairwayDoorTL1;
    public GameObject StairwayDoorTL2;
    public GameObject StairwayDoorBL1;
    public GameObject StairwayDoorBL2;
    [Header("Office Doors")]
    public GameObject NarcomancerDoor1;
    public GameObject NarcomancerDoor2;
    [Header("Front Doors")]
    public GameObject frontdoor1;
    public GameObject frontdoor2;

    public GameObject narrativecontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void OpenStairs()
    {
        StairwayDoorBL1.SendMessage("OpenDoors");
        StairwayDoorBL2.SendMessage("OpenDoors");
        StairwayDoorTL1.SendMessage("OpenDoors");
        StairwayDoorTL2.SendMessage("OpenDoors");
        StairwayDoorBR1.SendMessage("OpenDoors");
        StairwayDoorBR2.SendMessage("OpenDoors");
        StairwayDoorTR1.SendMessage("OpenDoors");
        StairwayDoorTR2.SendMessage("OpenDoors");
    }

    public void OpenOffice()
    {
        NarcomancerDoor1.SendMessage("OpenDoors");
        NarcomancerDoor2.SendMessage("OpenDoors");
        narrativecontroller.GetComponent<NarrativeEventManager>().OfficeDoors();
        print("the office door opens");
    }

    public void CloseOffice()
    {
        NarcomancerDoor1.SendMessage("CloseDoors");
        NarcomancerDoor2.SendMessage("CloseDoors");
    }

    public void OpenFront()
    {
        frontdoor1.SendMessage("OpenDoors");
        frontdoor2.SendMessage("OpenDoors");
        narrativecontroller.GetComponent<NarrativeEventManager>().DoorKick();
    }

    public void CloseFront()
    {
        frontdoor1.SendMessage("CloseDoors");
        frontdoor2.SendMessage("CloseDoors");
    }




}
