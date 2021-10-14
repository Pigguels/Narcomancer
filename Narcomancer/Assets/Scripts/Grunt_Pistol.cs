using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Grunt_Pistol : MonoBehaviour
{
    NavMeshAgent m_navAgent;

    public GameObject playerTarget;
    //public float rangeCheck = 5;
    public LayerMask playerLayer;
    public Transform spawnpoint;
    public GameObject bulletPrefab;
    Animator anim;

    //get the health scripts
    Health m_Health;

    //States

    public float m_sightRange, m_attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool trackingplayer = false;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    private void Awake()
    {
        m_Health = GetComponent<Health>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.stoppingDistance = m_attackRange;
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        m_navAgent.updatePosition = true;
        m_navAgent.stoppingDistance = m_attackRange;
        m_navAgent.destination = playerTarget.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        if (trackingplayer)
        {
            m_navAgent.SetDestination(playerTarget.transform.position);
            Vector3 rot = Quaternion.LookRotation(playerTarget.transform.position - transform.position).eulerAngles;
            rot.x = rot.z = 0;
            transform.rotation = Quaternion.Euler(rot);
            

            //this is me testing other rotation methods
            //Quaternion lookOnLook = Quaternion.LookRotation(playerTarget.transform.position - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.40f);
        }
        //every frame do distance checks to see if in range
        playerInSightRange = Vector3.Distance(transform.position, playerTarget.transform.position) < m_sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, playerTarget.transform.position) < m_attackRange;

        if (m_navAgent.velocity.x > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);
      

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();

        //test against if the character is dead
        if(m_Health.m_IsDead)
        {
            //play animation for death
            //destroy after animation
            anim.SetTrigger("Dead");
            Destroy(gameObject, 2f);    
        }
    }

    public void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            StartCoroutine(FireAtPlayer());
        }
    }
    public void ChasePlayer()
    {
        m_navAgent.SetDestination(playerTarget.transform.position);
        trackingplayer = true;

    }


    private IEnumerator FireAtPlayer()
    {
        anim.SetTrigger("PistolFire");
        alreadyAttacked = true;
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, spawnpoint.position, Quaternion.Euler(transform.forward));
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(spawnpoint.forward * 1000f);
        Destroy(bullet, 1.5f);
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

   
    void OnDrawGizmos()
    {
        

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, m_sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }


    private void OnDestroy()
    {
        //lootPickup.SpawnPickup();
    }
}