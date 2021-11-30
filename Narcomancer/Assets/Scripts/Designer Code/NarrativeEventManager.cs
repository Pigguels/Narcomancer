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
    public Animator henchmanAnim;
    public GameObject Briefcasereal;
    public GameObject Briefcasefake;


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
    public GameObject drugsprompt;
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
    private float volume;
    public float combat;

    private Animator narcomancerAnim;
    private Animator NarcoCageAnim;
    
    public Animator cageAnim;
    





    // Start is called before the first frame update
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        music.start();
        volume = 1;

        narcomancerAnim = OfficeNarcomancer.GetComponent<Animator>();
        //henchmanAnim = henchman.GetComponent<Animator>();
        NarcoCageAnim = CageNarcomancer.GetComponent<Animator>();
        cageAnim = glassCage.GetComponent<Animator>();
        henchmanAnim.SetBool("isPistol", true);
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
        music.setParameterByName("volume", volume);

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
        doorController.GetComponent<DoorController>().CloseOffice();
    }
    public void StoryOnThePhone()
    {
        phonering.SetActive(false);
        speakerParent.GetComponent<DialogueSpeaker>().PhoneDialogue();
        Briefcasefake.SetActive(true);
        drugsprompt.SetActive(true);
    }
    public void StoryDestroyTheDrugs()
    {
        StartCoroutine(BurnBaby());
    }

    public void StoryBossFight()
    {
        print("ci esta boss fight");
        barktimer = 40f;
        boss = 2;
        volume = 2;
        speakerParent.GetComponent<DialogueSpeaker>().BossFightStart();
        WaveMaster.GetComponent<WaveManager>().Boss1();
        CageNarcomancer.GetComponent<NarcoGrenade>().enabled = true;
        
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
        CageNarcomancer.GetComponent<NarcoGrenade>().enabled = false;
        boss = 0;
        volume = 0;

    }
    public void StoryMonologue()
    {
        speakerParent.GetComponent<DialogueSpeaker>().Monologue();
        monologueing = true;
        timer = 272f;
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
        narcomancerAnim.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.7f);
        VIPgunshot.SetActive(true);
        henchmanAnim.SetBool("Dead",true);
        yield return new WaitForSeconds(1f);
        speakerParent.GetComponent<DialogueSpeaker>().VipAudio2();
        doorController.GetComponent<DoorController>().OpenVIP();
        WaveMaster.GetComponent<WaveManager>().PostEnforcer();
        officeArrivalTrigger.SetActive(true);
        combat = 1;
        barktimer = 40f;
    }

    IEnumerator BossStart()
    {
        speakerParent.GetComponent<DialogueSpeaker>().DestroyedDrugs();
        yield return new WaitForSeconds(6.4f);
        doorController.GetComponent<DoorController>().OpenHatch();
        yield return new WaitForSeconds(.5f);
        cageAnim.SetTrigger("LowerCage");
        speakerParent.GetComponent<DialogueSpeaker>().Descent();
        boss = 1;
        volume = 2;
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
        CageNarcomancer.tag = "Untagged";
    }

    public void BadEnd()
    {
        StartCoroutine(Bribe());
    }

    public void GameEnd()
    {
        GameEndUI.SetActive(true);
        monologueing = false;
        speakerParent.GetComponent<DialogueSpeaker>().MonolgueEnd();

    }

    IEnumerator Bribe()
    {
        BadEndUI.SetActive(true);
        Briefcasereal.SetActive(false);
        //Briefcasefake.SetActive(true);
        yield return new WaitForSeconds(10f);
        BadEndUI.SetActive(false);
        
    }

   
}
