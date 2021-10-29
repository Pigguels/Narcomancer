using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{   
    [Header ("Controller Objects")]
    public GameObject WaveMaster;
    public GameObject doorController;
        
    [Header("Animation Objects")]
    public GameObject narcomancer;
    public GameObject henchman;
    public GameObject officeWindow;
    public GameObject glassCage;

    [Header("Audio Objects")]
    public GameObject speakerParent;
    public GameObject phonering;
    public GameObject phonedialogue1;
    public GameObject phonedialogue2;


    [Header("Trigger Objects")]
    public GameObject officeArrivalTrigger;
    public GameObject officeEscapeTrigger;
    public GameObject monologueTrigger;

    public GameObject officeGas;

    public float timer;
    private bool bluster;
    private bool timerenabled;

    public bool wave2;
    public bool wave3;
    public bool wave4;
    private bool bossarrival;

    private FMOD.Studio.EventInstance music;
    [FMODUnity.EventRef]
    public string fmodEvent;
    private float boss;
    public float combat;
    
    private Animator narcomancerAnim;
    private Animator windowAnim;
    private Animator henchmanAnim;
    public Animator cageAnim;


    // Start is called before the first frame update
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        music.start();

        narcomancerAnim = narcomancer.GetComponent<Animator>();
        windowAnim = officeWindow.GetComponent<Animator>();
        henchmanAnim = henchman.GetComponent<Animator>();
        cageAnim = glassCage.GetComponent <Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerenabled)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            timerenabled = false;
        }

        if (!timerenabled && wave2 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave2();
            doorController.GetComponent<DoorController>().OpenStairs();
            combat = 1;
            wave2 = false;
        }
        if (!timerenabled && wave3 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave3();
            combat = 1;
            wave3 = false;
        }
        if (!timerenabled && wave4 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave4();
            combat = 1;
            wave4 = false;
        }

        if (!timerenabled && bluster == true)
        {
            NarcoArrival();
            bluster = false;
        }

        if (!timerenabled && bossarrival == true)
        {
            OfficeEscape();
            bossarrival = false;
        }

        music.setParameterByName("combat", combat);
        music.setParameterByName("boss", boss);

    }

    public void StoryIntroduction()
    {
        print("wave 1 dialogue");
        WaveMaster.GetComponent<WaveManager>().Wave1();
        speakerParent.GetComponent<DialogueSpeaker>().ArrivalAudio();
        combat = 1;   
    }
    public void StoryWave2()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave2Audio();
        timer = 12.7f;
        timerenabled = true;
        wave2 = true;       
    }

    public void StoryWave3()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave3Audio();
        timer = 14f;
        timerenabled = true;
        wave3 = true;
    }
    public void StoryWave4()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave4Audio();
        timer = 15f;
        timerenabled = true;
        wave4 = true;
    }

    public void StoryVipRoom()
    {
        //fmod play VIPROOM
        //anim narco shoot henchman
        //anim henchman dies
        //wavecontroller.postenforcer 
        //officeArrivalTrigger.SetActive
    }
    public void StoryPhoneCall()
    {
        phonering.SetActive(true);
    }
    public void StoryOnThePhone()
    {
        phonering.SetActive(false);
        speakerParent.GetComponent<DialogueSpeaker>().PhoneDialogue();
       // phonedialogue1.SetActive(true);
    }
    public void StoryDestroyTheDrugs()
    {
        phonedialogue2.SetActive(true);
        timer = 6.4f;
        bluster = true;
        print("destroyed");
    }

    public void NarcoArrival()
    {
        print("lowercage");
        cageAnim.SetTrigger("LowerCage");
        boss = 1;
        timer = 20f;
        timerenabled = (true);
        bossarrival = true;
        //narcodialogue arrival
     
    }
    public void OfficeEscape()
    {
        officeEscapeTrigger.SetActive(true);
        //narcomanceranim.setrigger.shoot
        //Officeglass.shootingouttheglass
        officeGas.SetActive(true);
    }
    public void StoryBossFight()
    {
       // speakerParent.GetComponent<DialogueSpeaker>().BossFightstart();
        //wavcontroller.bossWave1
        boss = 2;
    }

    public void NarcomancerDeated()
    {

        //narcomanceranim.settrigger dying
        //cageani.whatever
        monologueTrigger.SetActive(true);
        boss = 2;


    }
    public void StoryMonologue()
    {
        //fmod monologue
        //narcomanim.settrigger monolgoue
    }
    

   
}
