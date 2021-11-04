using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActiveAI : MonoBehaviour
{
    public enum EnemyPrefabName { Rat, Grunt, Smg, Magnum, Enforcer, }
    public EnemyPrefabName AiName;
    public enum WhichWaveisThis { wave1, Wave4, postenforcer, }
    public WhichWaveisThis wavename;
    public GameObject Wave1Controller;
    public GameObject Wave4Controller;
    public GameObject Postencontroller;
    public GameObject RatAi;
    public GameObject GruntAi;
    public GameObject SmgAi;
    public GameObject MagnumAi;
    public GameObject EnforcerAi;
    public Health m_Health;
    // Start is called before the first frame update
    void Start()
    {
        Wave1Controller = GameObject.Find("Wave1Controller");
        Wave4Controller = GameObject.Find("Wave4Controller");
        Postencontroller = GameObject.Find("PostEnforcerController");
        switch (wavename)
        {
            case WhichWaveisThis.wave1:  
                gameObject.transform.parent = Wave1Controller.transform;
                break;
            case WhichWaveisThis.postenforcer:
                gameObject.transform.parent = Postencontroller.transform;
                break;
            case WhichWaveisThis.Wave4:
                gameObject.transform.parent = Wave4Controller.transform;
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Health.m_IsDead)
        {
            gameObject.SetActive(false);
        }
    }

    public void SpawnActive()
    {
        switch (AiName)
        {
            case EnemyPrefabName.Rat:
                print("i summon rat");
                Instantiate(RatAi,transform.position,transform.rotation);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Grunt:
                Instantiate(GruntAi, transform.position, transform.rotation);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Smg:
                SmgAi.SetActive(true);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Magnum:
                MagnumAi.SetActive(true);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Enforcer:
                EnforcerAi.SetActive(true);
                gameObject.SetActive(false);
                break;
        }
    }
}
