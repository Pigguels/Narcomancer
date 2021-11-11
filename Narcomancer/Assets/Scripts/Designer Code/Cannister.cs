using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : MonoBehaviour
{
    [Header("Active Cannisters")]
    public GameObject Active1;
    public GameObject Active2;
    public GameObject Active3;
    [Header("Inert Cannisters")]
    public GameObject Inert1;
    public GameObject Inert2;
    public GameObject Inert3;
    [Header("Cage Condition Objects")]
    public GameObject BrokenObjects; 
    public GameObject Glass;
    public Material CrackedCage1;
    public Material CrackedCage2;
    public Material CrackedCage3;
    public GameObject CrackSound;
    [Header("Gases")]
    public GameObject Gas;
    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;

  


    public GameObject Narocmancer;
    public GameObject Cage;
   
    public GameObject NarrativeEventManager;

    private Animator NarcoAnim;
    private Animator CageAnim;

    // Start is called before the first frame update
    void Start()
    {
        NarcoAnim = Narocmancer.GetComponent<Animator>();
        CageAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cannistor1Active()
    {
        Inert1.SetActive(false);
        Active1.SetActive(true);
        Gas.SetActive(true);
    }
    public void Cannistor1Destroyed()
    {
        print("youhitthecan");
        StartCoroutine(Can1());
    }

    public void Cannistor2Active()
    {
        Inert2.SetActive(false);
        Active2.SetActive(true);
        Gas.SetActive(true);
    }
    public void Cannistor2Destroyed()
    {
        StartCoroutine(Can2());
    }

    public void Cannistor3Active()
    {
        Inert3.SetActive(false);
        Active3.SetActive(true);
        Gas.SetActive(true);
    }
    public void Cannistor3Destroyed()
    {
        Explosion3.SetActive(true);
        Glass.GetComponent<Renderer>().material = CrackedCage3;
        Active3.SetActive(false);
        Gas.SetActive(false);
        NarcoAnim.SetBool("GlassBroken", true);
        NarcoAnim.SetTrigger("Surprised");
        CageAnim.SetTrigger("CageFall");
        NarrativeEventManager.GetComponent<NarrativeEventManager>().NarcomancerDefeated();
    }

    public void BreakObjects()
    {
        BrokenObjects.SetActive(true);
        Cage.SetActive(false);
        
    }
    IEnumerator Can1()
    {
        print("so wheres the boom");
        Explosion1.SetActive(true);
        Active1.SetActive(false);
        Gas.SetActive(false);
        NarcoAnim.SetTrigger("Surpirsed");
        CrackSound.SetActive(true);
        CrackSound.SetActive(false);
        Glass.GetComponent<Renderer>().material = CrackedCage1;
        yield return new WaitForSeconds(3f);
        NarrativeEventManager.GetComponent<NarrativeEventManager>().Boss2();
    }
    IEnumerator Can2()
    {
        Explosion2.SetActive(true);
        Active2.SetActive(false);
        Gas.SetActive(false);
        NarcoAnim.SetTrigger("Surpirsed");
        CrackSound.SetActive(true);
        CrackSound.SetActive(false);
        Glass.GetComponent<Renderer>().material = CrackedCage2;
        yield return new WaitForSeconds(3f);
        NarrativeEventManager.GetComponent<NarrativeEventManager>().Boss3();
    }
}
