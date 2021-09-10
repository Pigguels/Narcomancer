using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIBehaviour : MonoBehaviour
{
    NavMeshAgent m_navAgent;
 
    public GameObject playerTarget;
    public float rangeCheck = 5;
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

            Vector3 rot = Quaternion.LookRotation(playerTarget.transform.position - transform.position).eulerAngles;
            rot.x = rot.z = 0;
            transform.rotation = Quaternion.Euler(rot);

            //Quaternion lookOnLook = Quaternion.LookRotation(playerTarget.transform.position - transform.position);
            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.40f);

        }

       
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

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

    /* /// <summary>
     /// This is the initial testing class for the AI States and will be working with the variables from here.
     /// will be looking to clean up the scripts or create clean ones labeled for the specific AI characters
     /// </summary>
     NavMeshAgent m_navAgent;
     CharacterController m_charCont;

     public Transform spawnpoint;
     public GameObject bulletPrefab;
     private Vector3 m_destination;
     private float m_distance;
     private Quaternion m_desiredRotation;
     private Vector3 m_direction;
     private AIState m_currentState;
     private float m_stoppingDistance = 1.5f;
     private float m_rayDistance = 5.0f;
     public GameObject m_target;
     public float moveSpeed = 15;
     private bool hasFired;

     private float m_sightRange = 5.0f;
     private float m_attackRange = 5.0f;
     public LayerMask playerLayer;





     // Start is called before the first frame update
     void Start()
     {
         m_charCont = GetComponent<CharacterController>();
         m_navAgent = GetComponent<NavMeshAgent>();

         m_navAgent.updatePosition = true;
     }

     private IEnumerator Attack()
     {
         hasFired = true;
         GameObject bullet;
         bullet = Instantiate(bulletPrefab, spawnpoint.position, Quaternion.identity);
         bullet.gameObject.GetComponent<Rigidbody>().AddForce(spawnpoint.forward * 1000f);
         Destroy(bullet, 1.5f);
         yield return new WaitForSeconds(.2f);
         hasFired = false;
     }

     // Update is called once per frame
     void Update()
     {

         m_distance = Vector3.Distance(m_target.transform.position, transform.position);
         // transform.LookAt(m_target.transform, Vector3.up);
         //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

         Collider[] hits = Physics.OverlapSphere(transform.position, m_sightRange);
         foreach (Collider hit in hits)
         {
             if (hit.tag == "Player")
             { 
                 Transform target = hit.transform;
                 Vector3 dirToTarget = (target.position - transform.position).normalized;
                 if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                 {
                     float dstToTarget = Vector3.Distance(transform.position, target.position);
                     if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget))
                     {

                     }
                 }

             }
         }
         //Gizmos.DrawWireSphere(transform.position, m_sightRange);





         if (m_distance > 4)
         {
             m_navAgent.updatePosition = true;
             m_navAgent.SetDestination(m_target.transform.position);

         }
         if (m_distance <= 4)
         {
             m_navAgent.updatePosition = false;
             if (!hasFired)
                 StartCoroutine(Attack());

         }
 */





    //Vector3 direction = (m_navAgent.nextPosition - transform.position);
    ////transform.position = m_navAgent.nextPosition;
    //Debug.DrawLine(transform.position, m_navAgent.nextPosition,Color.red);
    //m_charCont.Move(direction * moveSpeed * Time.deltaTime);
    //
    //if (Keyboard.current.tabKey.isPressed)
    //{
    //
    //}

    /*
    idle / in position
    chase / get in position
    attacking

    if idling
        check if player is within a certain range 

    */

    /*switch (m_currentState)
    {
        case AIState.Wander:
            {
                if (NeedsDestination())
                {
                    GetDestination();
                }

                transform.rotation = m_desiredRotation;

                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                var rayColor = IsPathBlocked() ? Color.red : Color.green;
                Debug.DrawRay(transform.position, m_direction * m_rayDistance, rayColor);

                while (IsPathBlocked())
                {
                    Debug.Log("Path Blocked");
                    GetDestination();
                }

                if (Physics.CheckSphere(transform.position, m_sightRange, playerLayer))
                {
                    m_target = GameObject.FindGameObjectWithTag("Player");
                    m_currentState = AIState.Chase;
                }



                break;
            }
        case AIState.Chase:
            {
                if (m_target == null)
                {
                    m_currentState = AIState.Wander;
                    return;
                }

                transform.LookAt(m_target.transform);
                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, m_target.transform.position) < m_attackRange)
                {
                    m_currentState = AIState.Attack;
                }

                break;
            }
        case AIState.Attack:
            {


                m_currentState = AIState.Wander;
                break;
            }
    }*/


   /* private bool NeedsDestination()
    {
        if (m_destination == null)
            return true;

        var distance = Vector3.Distance(transform.position, m_destination);
        if (distance <= m_stoppingDistance)
        {
            return true;
        }

        return false;
    }

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) + new Vector3(Random.Range(-4.5f, 4.5f), 0f, Random.Range(-4.5f, 4.5f));

        m_destination = new Vector3(testPosition.x, 1f, testPosition.z);

        m_direction = Vector3.Normalize(m_destination - transform.position);
        m_direction = new Vector3(m_direction.x, 0f, m_direction.z);
        m_desiredRotation = Quaternion.LookRotation(m_direction);
    }
    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, m_direction);
        var hitSomething = Physics.Raycast(ray, m_rayDistance, playerLayer);
        return hitSomething;
    }
*/


}

public enum AIState
{
    Wander,
    Chase,
    Attack
}