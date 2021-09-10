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


    private float m_distance;
    public GameObject m_target;
    public float moveSpeed = 15;
    private bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.stoppingDistance = m_attackRange;
        m_navAgent.updatePosition = true;
    }

    // Update is called once per frame 
    void Update()
    {
        
        m_navAgent.Move(transform.forward * Time.deltaTime);
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

    }
    private IEnumerator Attack()
    {
        hasAttacked = true;

        yield return new WaitForSeconds(m_attackRate);
        Debug.Log("Rat Attack");
        hasAttacked = false;
    }
}
