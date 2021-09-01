using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt_Magnum : MonoBehaviour
{
    NavMeshAgent m_navAgent;

    public GameObject playerTarget;
    //public float rangeCheck = 5;
    public LayerMask playerLayer;
    public Transform spawnpoint;
    public GameObject bulletPrefab;



    //States

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool trackingplayer = false;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;



    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.stoppingDistance = attackRange;

        m_navAgent.updatePosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingplayer)
        {
            m_navAgent.SetDestination(playerTarget.transform.position);
            Quaternion lookOnLook = Quaternion.LookRotation(playerTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.40f);
        }


        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);


        playerInSightRange = Vector3.Distance(transform.position, playerTarget.transform.position) < sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, playerTarget.transform.position) < attackRange;

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();

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
        alreadyAttacked = true;
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, spawnpoint.position, Quaternion.identity);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(spawnpoint.forward * 1000f);
        Destroy(bullet, 1.5f);
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }
}