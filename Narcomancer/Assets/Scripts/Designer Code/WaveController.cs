using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public enum WaveControllerNumber { Wave1, Wave2, Wave3, Wave4, Boss1, Boss2, Boss3,}
    public WaveControllerNumber waveno;
    public GameObject NarrativeController;
    public GameObject CannisterMaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndOfWave()
    {
        NarrativeController.GetComponent<NarrativeEventManager>().combat = 0;
        switch (waveno)
        {
            case WaveControllerNumber.Wave1:
                NarrativeController.GetComponent<NarrativeEventManager>().StoryWave2();
                break;
            case WaveControllerNumber.Wave2:
                NarrativeController.GetComponent<NarrativeEventManager>().StoryWave3();
                break;
            case WaveControllerNumber.Wave3:
                NarrativeController.GetComponent<NarrativeEventManager>().StoryWave4();
                break;
            case WaveControllerNumber.Wave4:
                NarrativeController.GetComponent<NarrativeEventManager>().StoryVipRoom();
                break;
            case WaveControllerNumber.Boss1:
                CannisterMaster.GetComponent<Cannister>().Cannistor1Active();
                break;
            case WaveControllerNumber.Boss2:
                CannisterMaster.GetComponent<Cannister>().Cannistor2Active();
                break;
            case WaveControllerNumber.Boss3:
                CannisterMaster.GetComponent<Cannister>().Cannistor3Active();
                break;

        }

    }

}
