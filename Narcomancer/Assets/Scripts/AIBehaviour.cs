using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{

    NavMeshAgent m_navAgent;
    private Vector3 m_destination;
    private Quaternion m_desiredRotation;
    private Vector3 m_direction;
    private AIState m_currentState;
    private float m_stoppingDistance = 1.5f;



    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentState)
        {
            case AIState.Wander:
                {


                    break;
                }
            case AIState.Chase:
                {
                    break;
                }
            case AIState.Attack:
                {
                    break;
                }
        }
    }

    private bool NeedsDestination()
    {
        if (m_destination == Vector3.zero)
            return true;

        var distance = Vector3.Distance(transform.position, m_destination);
        if (distance <= m_stoppingDistance)
        {
            return true;
        }

        return false;
    }



}

public enum AIState
{
    Wander,
    Chase,
    Attack
}