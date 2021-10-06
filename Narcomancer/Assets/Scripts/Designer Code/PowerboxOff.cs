using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerboxOff : MonoBehaviour
{
    public GameObject lightController;
    public GameObject switchObject;
    public GameObject OfficeDoor;
    public float powerResetTimer;
    private float powerResetTimerActual;
    // Start is called before the first frame update
    void Awake()
    {
        lightController.PowerOff();
        OfficeDoor.GetComponent<DoorAnimation>().OpenDoors();
        powerResetTimerActual = powerResetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (powerResetTimerActual <= 0f)
        {
            switchObject.SetActive(true);
            lightController.PowerOn();
            OfficeDoor.GetComponent<DoorAnimation>().CloseDoors();
            gameObject.SetActive(false);
        }
    }
}
