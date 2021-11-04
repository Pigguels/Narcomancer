using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{   
    [Header ("Controller Objects")]
    public GameObject WaveMaster;
    public GameObject doorController;
        
    [Header("Animation Objects")]
    public GameObject OfficeNarcomancer;
    public GameObject CageNarcomancer;
    public GameObject henchman;
    public GameObject officeWindow;
    public GameObject glassCage;

    [Header("Audio Objects")]
    public GameObject speakerParent;
    public GameObject phonering;
    public GameObject OfficeBark;
    public GameObject BossBark;
    //public GameObject Phonedialogue;
    //public GameObject PhoneScreaming;


    [Header("Trigger Objects")]
    public GameObject officeArrivalTrigger;
    public GameObject officeEscapeTrigger;
    public GameObject monologueTrigger;

    public GameObject officeGas;

    public float timer;
    public float barktimer;
    public bool bluster;
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
    private Animator NarcoCageAnim;
    private Animator windowAnim;
    private Animator henchmanAnim;
    public Animator cageAnim;
    


    // Start is called before the first frame update
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        music.start();

        narcomancerAnim = OfficeNarcomancer.GetComponent<Animator>();
        windowAnim = officeWindow.GetComponent<Animator>();
        henchmanAnim = henchman.GetComponent<Animator>();
        NarcoCageAnim = CageNarcomancer.GetComponent<Animator>();
        cageAnim = glassCage.GetComponent <Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // timer for "Barks"
        if (combat == 1 || boss == 2)
        {
            barktimer -= Time.deltaTime;
        }
        //Activate Wave Barls
        if (barktimer <= 0f && combat == 1)
        {
            OfficeBark.SetActive(false);
            barktimer = 30f;
            OfficeBark.SetActive(true);
        }
        if (combat == 0)
        {
            OfficeBark.SetActive(false);
        }
        //Stop Bark midway

        //Activate Boss Barks
        if (barktimer <= 0f && boss == 2)
        {
            BossBark.SetActive(false);
            barktimer = 30f;
            BossBark.SetActive(true);
        }
        if (boss == 0)
        {
            BossBark.SetActive(false);
        }
        //Stop Bark midway

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
            narcomancerAnim.SetBool("Talking", false);
            combat = 1;
            wave2 = false;
        }
        if (!timerenabled && wave3 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave3();
            narcomancerAnim.SetBool("Talking", false);
            combat = 1;
            wave3 = false;
        }
        if (!timerenabled && wave4 == true)
        {
            WaveMaster.GetComponent<WaveManager>().Wave4();
            narcomancerAnim.SetBool("Talking", false);
            combat = 1;
            wave4 = false;
        }

        music.setParameterByName("combat", combat);
        music.setParameterByName("boss", boss);

    }

    public void DoorKick()
    {
        combat = 1;
        barktimer = 30f;
    }
    public void StoryIntroduction()
    {
        WaveMaster.GetComponent<WaveManager>().Wave1();
        speakerParent.GetComponent<DialogueSpeaker>().ArrivalAudio();
        barktimer = 30f;
        StartCoroutine(closefrontdoors());
    }
    public void StoryWave2()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave2Audio();
        timer = 12.7f;
        timerenabled = true;
        narcomancerAnim.SetBool("Talking",true);
        wave2 = true;       
    }

    public void StoryWave3()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave3Audio();
        timer = 14f;
        timerenabled = true;
        narcomancerAnim.SetBool("Talking", true);
        wave3 = true;
    }
    public void StoryWave4()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave4Audio();
        timer = 15f;
        timerenabled = true;
        narcomancerAnim.SetBool("Talking", true);
        wave4 = true;
    }

    public void StoryVipRoom()
    { 
        StartCoroutine(VIP());
    }

    public void ShootTheMessenger()
    {
        narcomancerAnim.SetTrigger("Shoot");
        henchmanAnim.SetTrigger("Dead");
    }

    public void OfficeDoors()
    {
       // WaveMaster.GetComponent<WaveManager>().AlarmRatsStart();
        OfficeNarcomancer.SetActive(false);
    }

    public void StoryPhoneCall()
    {
        print("phonerings");
        //WaveMaster.GetComponent<WaveManager>().AlarmRatsStop();
        phonering.SetActive(true);
    }
    public void StoryOnThePhone()
    {
        phonering.SetActive(false);
        speakerParent.GetComponent<DialogueSpeaker>().PhoneDialogue();
    }
    public void StoryDestroyTheDrugs()
    {
        StartCoroutine(BossStart());
        print("destroyeddrugs");
    }

    public void StoryBossFight()
    {
        speakerParent.GetComponent<DialogueSpeaker>().BossFightStart();
        WaveMaster.GetComponent<WaveManager>().Boss1();
        boss = 2;
    }

    public void Boss2()
    {
        WaveMaster.GetComponent<WaveManager>().Boss2();
        BossBark.SetActive(false);
        BossBark.SetActive(true);
    }

    public void Boss3()
    {
        WaveMaster.GetComponent<WaveManager>().Boss3();
        BossBark.SetActive(false);
        BossBark.SetActive(true);
    }

    public void NarcomancerDefeated()
    {
       
       // cageAnim.SetTrigger("CageFall");
        StartCoroutine(triggerDelay());
        boss = 0;


    }
    public void StoryMonologue()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Monologue();
        //narcomanim.settrigger monolgoue
    }
    
    IEnumerator VIP()
    {
        speakerParent.GetComponent<DialogueSpeaker>().VipAudio();
        yield return new WaitForSeconds(37f);
        speakerParent.GetComponent<DialogueSpeaker>().HenchAudio();
        yield return new WaitForSeconds(10f);
        ShootTheMessenger();
        yield return new WaitForSeconds(10f);
        speakerParent.GetComponent<DialogueSpeaker>().VipAudio2();
        WaveMaster.GetComponent<WaveManager>().PostEnforcer();
        officeArrivalTrigger.SetActive(true);
        combat = 1;
    }

    IEnumerator BossStart()
    {
        speakerParent.GetComponent<DialogueSpeaker>().DestroyedDrugs();
        yield return new WaitForSeconds(6.4f);
        cageAnim.SetTrigger("LowerCage");
        speakerParent.GetComponent<DialogueSpeaker>().Descent();
        boss = 1;
        yield return new WaitForSeconds(20f);
        officeEscapeTrigger.SetActive(true);
        NarcoCageAnim.SetTrigger("Shoot");
        //Officeglass.shootingouttheglass
        officeGas.SetActive(true);
    }

    IEnumerator triggerDelay()
    {
        yield return new WaitForSeconds(3f);
        monologueTrigger.SetActive(true);
    }
    IEnumerator closefrontdoors()
    {
        yield return new WaitForSeconds(10f);
       // print("frontdoorsclose");
        doorController.GetComponent<DoorController>().CloseFront();
    }

}
