using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIBehaviour : MonoBehaviour
{

    NavMeshAgent m_navAgent;
    CharacterController m_charCont;


    private Vector3 m_destination;
    private Quaternion m_desiredRotation;
    private Vector3 m_direction;
    private AIState m_currentState;
    private float m_stoppingDistance = 1.5f;
    private float m_rayDistance = 5.0f;
    public GameObject m_target;
    public float moveSpeed = 15;


    private float m_sightRange = 5.0f;
    private float m_attackRange = 5.0f;
    private LayerMask playerLayer;





    // Start is called before the first frame update
    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
        m_navAgent = GetComponent<NavMeshAgent>();
       
        m_navAgent.updatePosition = false;
    }


    // Update is called once per frame
    void Update()
    {
        m_navAgent.SetDestination(m_target.transform.position);

      
        Vector3 direction = (m_navAgent.nextPosition - transform.position);
        //transform.position = m_navAgent.nextPosition;
        Debug.DrawLine(transform.position, m_navAgent.nextPosition,Color.red);
        m_charCont.Move(direction * moveSpeed * Time.deltaTime);

        if (Keyboard.current.tabKey.isPressed)
        {

        }

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
    }
    
    private bool NeedsDestination()
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



}

public enum AIState
{
    Wander,
    Chase,
    Attack
}