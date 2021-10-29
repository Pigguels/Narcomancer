﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    //enum States { idle, chase, flee, attack, stunned, statesCount };
    //private States m_CurrentState = States.idle;

    public float m_DistanceToStopFollowing = 6.5f;
    public float m_DistanceToFlee = 3f;
    [Space]
    public float m_MinDistanceToAttack = 3f;
    public float m_MaxDistanceToAttack = 6.5f;
    [Space]
    public float m_TimeBetweenAttacks = 3.25f;
    private float m_TimeUntilNextAttack = 0f;
    [Space]
    public int m_SubAttackAmount = 3;
    private int m_CurrentSubAttack = 0;
    public float m_TimeBetweenSubAttacks = 0.25f;
    private float m_TimeUntilNextSubAttack = 0f;
    public float m_ProjectileXBloom = 0f;
    public float m_ProjectileYBloom = 0f;
    public Transform m_ProjectileSpawnPos;

    [Header("Referances:")]
    public GameObject m_Projectile;
    public Transform m_PlayerPos;
    public Health m_Health;

    private NavMeshAgent m_NavAgent;

    // Start is called before the first frame update
    void Start()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Health.m_IsDead)
        {
            float sqrDistanceToPlayer = (m_PlayerPos.position - transform.position).sqrMagnitude;

            /* Are we too far from the player? */
            if (sqrDistanceToPlayer > m_DistanceToStopFollowing)
            {
                //follow player
                m_NavAgent.SetDestination(m_PlayerPos.position);
            }
            /* Are we too close to the player? */
            else if (sqrDistanceToPlayer < m_DistanceToFlee)
            {
                // flee from player
                m_NavAgent.SetDestination((transform.position - m_PlayerPos.position).normalized * m_DistanceToStopFollowing);
            }
            /* We are in range of the player, not to close not too far */
            else
            {
                // strafe needs to go here

                m_NavAgent.SetDestination(transform.position);
            }

            /* Attack */
            if (sqrDistanceToPlayer > m_MinDistanceToAttack && sqrDistanceToPlayer < m_MaxDistanceToAttack)
            {
                if (m_TimeUntilNextAttack <= 0f && Physics.Raycast(m_ProjectileSpawnPos.position, (m_PlayerPos.position - m_ProjectileSpawnPos.position).normalized, m_MaxDistanceToAttack, LayerMask.GetMask("Player")))
                {
                    Debug.Log(gameObject.name + " is attacking");

                    /* End attack */
                    if (m_CurrentSubAttack >= m_SubAttackAmount)
                    {
                        m_TimeUntilNextAttack = m_TimeBetweenAttacks + m_TimeBetweenSubAttacks * m_SubAttackAmount;
                        m_CurrentSubAttack = 0;
                    }
                    /* Do each sub attack */
                    else
                    {
                        /* Actual attack */
                        if (m_TimeUntilNextSubAttack <= 0f)
                        {
                            Debug.Log(gameObject.name + " fired projectile");
                            /* Spawn projectile aim it at the player */
                            // NEED TO NUKE THIS WHEN POOLING IS ADDED
                            GameObject spawnedProjectile = Instantiate(m_Projectile, m_ProjectileSpawnPos.position, Quaternion.LookRotation(m_PlayerPos.position - transform.position) * Quaternion.Euler(Random.Range(-m_ProjectileXBloom, m_ProjectileXBloom), Random.Range(-m_ProjectileYBloom, m_ProjectileYBloom), 0f));

                            //  NEED TO ADD MUZZEL FLASH GARBEGEDE HERE

                            /* Reset for next sub attack */
                            ++m_CurrentSubAttack;
                            m_TimeUntilNextSubAttack = m_TimeBetweenSubAttacks;
                        }
                        else
                            m_TimeUntilNextSubAttack -= Time.deltaTime;
                    }
                }
            }

            if (m_TimeUntilNextAttack > 0f)
                m_TimeUntilNextAttack -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // NEED TO NUKE THIS FOR WHEN POOLING COMES
        }
    }
}