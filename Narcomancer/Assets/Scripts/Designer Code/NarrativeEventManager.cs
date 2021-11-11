using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{
    [Header("Controller Objects")]
    public GameObject WaveMaster;
    public GameObject doorController;

    [Header("Animation Objects")]
    public GameObject OfficeNarcomancer;
    public GameObject CageNarcomancer;
    public GameObject henchman;
    public GameObject officeWindow;
    public GameObject officeWindowBroken;
    public GameObject glassCage;
    
    public GameObject GoodEndUI;
    public GameObject BadEndUI;
    public GameObject GameEndUI;

    [Header("Audio Objects")]
    public GameObject speakerParent;
    public GameObject phonering;
    public GameObject OfficeBark;
    public GameObject BossBark;
    public GameObject firealarm;
    public GameObject firealarmoff;
    public GameObject VIPgunshot;   
    public GameObject burnsound;
    //public GameObject Phonedialogue;
    //public GameObject PhoneScreaming;

    [Header("Trigger Objects")]
    public GameObject IntroTrigger;
    public GameObject VipTrigger;
    public GameObject officeArrivalTrigger;
    public GameObject officeEscapeTrigger;
    public GameObject monologueTrigger;

    public GameObject officeGas;
    public GameObject drugs;
    public GameObject destroyeddrugs;

    public float timer;
    public float barktimer;
    public bool bluster;
    private bool timerenabled;
    private bool monologueing;
    public bool wave2;
    public bool wave3;
    public bool wave4;
    //private bool bossarrival;

    private FMOD.Studio.EventInstance music;
    [FMODUnity.EventRef]
    public string fmodEvent;
    private float boss;
    private float casual;
    public float combat;

    private Animator narcomancerAnim;
    private Animator NarcoCageAnim;
    private Animator henchmanAnim;
    public Animator cageAnim;
    





    // Start is called before the first frame update
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        music.start();

        narcomancerAnim = OfficeNarcomancer.GetComponent<Animator>();
        henchmanAnim = henchman.GetComponent<Animator>();
        NarcoCageAnim = CageNarcomancer.GetComponent<Animator>();
        cageAnim = glassCage.GetComponent<Animator>();
        
        monologueing = false;
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
            doorController.GetComponent<DoorController>().OpenVIP();
            WaveMaster.GetComponent<WaveManager>().Wave4();
            narcomancerAnim.SetBool("Talking", false);
            combat = 1;
            wave4 = false;
        }
        if (!timerenabled && monologueing == true)
        {
            GoodEnd();
        }

        music.setParameterByName("combat", combat);
        music.setParameterByName("boss", boss);
        music.setParameterByName("casual", casual);

    }

    public void DoorKick()
    {
        barktimer = 40f;
        combat = 1;
        IntroTrigger.SetActive(true);
    }
    public void StoryIntroduction()
    {
        WaveMaster.GetComponent<WaveManager>().Wave1(); 
        speakerParent.GetComponent<DialogueSpeaker>().ArrivalAudio();
        barktimer = 40f;
        //StartCoroutine(closefrontdoors());
    }
    public void StoryWave2()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Wave2Audio();
        timer = 12.7f;
        timerenabled = true;
        narcomancerAnim.SetBool("Talking", true);
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
        VIPgunshot.SetActive(true);
        henchmanAnim.SetTrigger("Dead");

    }

    public void OfficeDoors()
    {
        // WaveMaster.GetComponent<WaveManager>().AlarmRatsStart();
        OfficeNarcomancer.SetActive(false);
        henchman.SetActive(false);
        firealarmoff.SetActive(false);
        firealarm.SetActive(true);
    }

    public void StoryPhoneCall()
    {
        firealarmoff.SetActive(true);
        firealarm.SetActive(false);
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
        StartCoroutine(BurnBaby());
    }

    public void StoryBossFight()
    {
        print("ci esta boss fight");
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

        //cageAnim.SetTrigger("CageFall");
        StartCoroutine(triggerDelay());
        boss = 0;
        casual = 1;

    }
    public void StoryMonologue()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Monologue();
        monologueing = true;
        timer = 280f;
        timerenabled = true;
        //na.settrigger monolgoue - ANIMATION DOESN NOT EXIST PRESENTLY

    }

    IEnumerator VIP()
    {
        VipTrigger.SetActive(true);
        speakerParent.GetComponent<DialogueSpeaker>().VipAudio();
        yield return new WaitForSeconds(37f);
        speakerParent.GetComponent<DialogueSpeaker>().HenchAudio();
        yield return new WaitForSeconds(6f);
        ShootTheMessenger();
        yield return new WaitForSeconds(5f);
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
        officeWindow.SetActive(false);
        officeWindowBroken.SetActive(true);
        officeGas.SetActive(true);
    }

    IEnumerator BurnBaby()
    {
        burnsound.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        drugs.SetActive(false);
        destroyeddrugs.SetActive(true);
        StartCoroutine(BossStart());
        print("destroyeddrugs");
    }
    IEnumerator triggerDelay()
    {
        yield return new WaitForSeconds(3f);
        monologueTrigger.SetActive(true);
    }
    //IEnumerator closefrontdoors()
    //{
    //    yield return new WaitForSeconds(10f);
    //    doorController.GetComponent<DoorController>().CloseFront();
    //}

    public void GoodEnd()
    {
        GoodEndUI.SetActive(true);
    }

    public void BadEnd()
    {
        BadEndUI.SetActive(true);
    }

    public void GameEnd()
    {
        GameEndUI.SetActive(true);
        monologueing = false;

    }
}
