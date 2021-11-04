using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActiveAI : MonoBehaviour
{
    public enum EnemyPrefabName { Rat, Grunt, Smg, Magnum, Enforcer, }
    public EnemyPrefabName AiName;
    public GameObject RatAi;
    public GameObject GruntAi;
    public GameObject SmgAi;
    public GameObject MagnumAi;
    public GameObject EnforcerAi;
    public Health m_Health;
    // Start is called before the first frame update
    void Start()
    {
      
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
