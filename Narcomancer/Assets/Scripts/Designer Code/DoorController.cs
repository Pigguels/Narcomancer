using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject StairwayDoorTR1;
    private GameObject StairwayDoorTR2;
    private GameObject StairwayDoorBR1;
    private GameObject StairwayDoorBR2;
    private GameObject StairwayDoorTL1;
    private GameObject StairwayDoorTL2;
    private GameObject StairwayDoorBL1;
    private GameObject StairwayDoorBL2;

    private GameObject NarcomancerDoor1;
    private GameObject NarcomancerDoor2;
    
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
        StairwayDoorBL1.SendMessage("OpenDoor");
        StairwayDoorBL2.SendMessage("OpenDoor");
        StairwayDoorTL1.SendMessage("OpenDoor");
        StairwayDoorTL2.SendMessage("OpenDoor");
        StairwayDoorBR1.SendMessage("OpenDoor");
        StairwayDoorBR2.SendMessage("OpenDoor");
        StairwayDoorTR1.SendMessage("OpenDoor");
        StairwayDoorTR2.SendMessage("OpenDoor");
    }

    public void OpenOffice()
    {
        NarcomancerDoor1.SendMessage("OpenDoor");
        NarcomancerDoor2.SendMessage("OpenDoor");
        print("the office door opens");
    }

}
