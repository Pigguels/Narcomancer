using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_AI : MonoBehaviour
{
    NavMeshAgent m_navAgent;
    //CharacterController m_charCont;  //this character controller could be used for manual ai movement if needed

    //the enemy attack rate
    public float m_attackRate = 0.5f;
    public float m_attackRange = 2;

    //get the health scripts
    Health m_Health;

    private float m_distance;
    public GameObject m_target;
    public float moveSpeed = 15;
    private bool hasAttacked;
    Animator anim;
    LootSpawner lootPickup;

    private void Awake()
    {
        m_Health = GetComponent<Health>();
        anim = GetComponentInChildren<Animator>();
        lootPickup = GetComponent<LootSpawner>();
    }


    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.stoppingDistance = m_attackRange;
        m_target = GameObject.FindGameObjectWithTag("Player");
        m_navAgent.updatePosition = true;
        m_navAgent.stoppingDistance = m_attackRange;
        m_navAgent.destination = m_target.transform.position;

    }

    // Update is called once per frame 
    void Update()
    {

        if (m_navAgent.velocity.x > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);
        //m_navAgent.Move(transform.right * Time.deltaTime);
        //m_navAgent.Move(transform.right * Time.deltaTime);

        m_distance = Vector3.Distance(m_target.transform.position, transform.position);
        transform.LookAt(m_target.transform, Vector3.up);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (m_distance > m_attackRange)
        {
            m_navAgent.updatePosition = true;
            m_navAgent.SetDestination(m_target.transform.position);

        }
        if (m_distance <= m_attackRange)
        {
            m_navAgent.updatePosition = false;
            if (!hasAttacked)
                StartCoroutine(Attack());

        }

        if (m_Health.m_IsDead)
        {
            anim.SetTrigger("Dead");
            Destroy(gameObject, 2f);
        }



    }
    private void OnDestroy()
    {


        lootPickup.SpawnPickup();
    }
    private IEnumerator Attack()
    {
        hasAttacked = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(m_attackRate);
        Debug.Log("Rat Attack");
        hasAttacked = false;
    }
}
