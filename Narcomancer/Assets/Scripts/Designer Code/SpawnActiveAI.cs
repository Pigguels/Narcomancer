using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActiveAI : MonoBehaviour
{
    public enum EnemyPrefabName { Rat, Grunt, Smg, Magnum, Enforcer, }
    public EnemyPrefabName AiName;
    public enum WhichWaveisThis { wave1, Wave4, postenforcer, }
    public WhichWaveisThis wavename;
    public GameObject RatAi;
    public GameObject GruntAi;
    public GameObject SmgAi;
    public GameObject MagnumAi;
    public GameObject EnforcerAi;
    private GameObject playerposition;
    public Health m_Health;
    // Start is called before the first frame update
    void Start()
    {
        playerposition = GameObject.Find("Player");       
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
                RatAi.GetComponent<EnemyAI>().m_PlayerPos = playerposition.transform;
                Instantiate(RatAi, transform.position, transform.rotation);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Grunt:
                Instantiate(GruntAi, transform.position, transform.rotation);
                GruntAi.GetComponent<EnemyAI>().m_PlayerPos = playerposition.transform;
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Smg:
                GruntAi.GetComponent<EnemyAI>().m_PlayerPos = playerposition.transform;
                SmgAi.SetActive(true);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Magnum:
                GruntAi.GetComponent<EnemyAI>().m_PlayerPos = playerposition.transform;
                MagnumAi.SetActive(true);
                gameObject.SetActive(false);
                break;
            case EnemyPrefabName.Enforcer:
                GruntAi.GetComponent<EnemyAI>().m_PlayerPos = playerposition.transform;
                EnforcerAi.SetActive(true);
                gameObject.SetActive(false);
                break;
        }
    }
}
