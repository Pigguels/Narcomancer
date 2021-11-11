using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    enum States { idle, chase, flee, strafe, attacking, staggered, stunned, dead, statesCount };
    private States m_CurrentState = States.idle;

    public enum EnemyType { gruntPistol, gruntMagnum, gruntSMG, enforcer, rat, typeCount };
    public EnemyType m_EnemyType = EnemyType.gruntPistol;

    public bool m_StartAlerted = false;
    public bool m_AlertOnSight = true;
    public float m_AlertDistance = 15f;
    [Space]
    public float m_DistanceToStopFollowing = 12f;
    public float m_DistanceToFlee = 4.5f;
    [Space]
    public float m_MinDistanceToAttack = 4f;
    public float m_MaxDistanceToAttack = 14f;
    [Space]
    public float m_MinTimeBetweenStrafes = 1f;
    public float m_MaxTimeBetweenStrafes = 5f;
    private float m_TimeUntilStrafeStart = 0f;
    public float m_MaxStrafeTime = 4f;
    public float m_MinStrafeTime = 0.5f;
    private float m_TimeUntilStrafeEnd = 0f;
    public float m_StrafeSpeed = 2.5f;
    private bool m_StrafeRight = true;
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
    [Space]
    public Vector3 m_LightningChainOffset = Vector3.zero;
    [Space]
    public float m_ModelAllignmentSpeed = 0.5f;
    private Vector3 m_TargetLookDir;
    private Vector3 towardPlayer2D;
    public Transform m_ModelParent;
    [Space]

    [Header("Referances:")]
    public GameObject m_Projectile;
    public Health m_Health;
    public Animator m_Animator;
    LootSpawner lootSpawner;

    public Transform m_PlayerPos;
    public EnemyWave m_WaveManager;

    private NavMeshAgent m_NavAgent;

    private LayerMask m_EnemyMask;

    void Awake()
    {
        if (m_StartAlerted)
            m_CurrentState = States.chase;
        else
            m_CurrentState = States.idle;

        m_NavAgent = GetComponent<NavMeshAgent>();
        lootSpawner = GetComponent<LootSpawner>();

        m_EnemyMask = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (PlayerController.paused)
            return;

        switch (m_CurrentState)
        {
            case States.idle:
                m_Animator.SetBool("isWalking", false);
                m_Animator.SetBool("Dead", false);

                /* Check if players in range and in view */
                if (m_AlertOnSight)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(m_ProjectileSpawnPos.position, m_PlayerPos.position - m_ProjectileSpawnPos.position, out hit, m_AlertDistance, ~m_EnemyMask))
                        if (hit.transform.CompareTag("Player"))
                            m_CurrentState = States.chase;
                }
                break;
            case States.chase:
                m_Animator.SetBool("Dead", false);
                break;
            case States.flee:
                m_Animator.SetBool("Dead", false);
                break;
            case States.strafe:
                m_Animator.SetBool("Dead", false);
                break;
            case States.attacking:
                m_Animator.SetBool("Dead", false);
                break;
            case States.staggered:
                m_Animator.SetBool("Dead", false);
                break;
            case States.stunned:
                m_Animator.SetBool("Dead", false);
                break;
            case States.dead:
                m_Animator.SetBool("Dead", true);
                break;
        }

        if (!m_Health.m_IsDead)
        {
            float sqrDistanceToPlayer = (m_PlayerPos.position - transform.position).sqrMagnitude;
            towardPlayer2D = new Vector3(m_PlayerPos.position.x - transform.position.x, 0f, m_PlayerPos.position.z - transform.position.z).normalized;

            if (m_CurrentState != States.idle)
            {
                /* Are we too far from the player? */
                if (sqrDistanceToPlayer > m_DistanceToStopFollowing * m_DistanceToStopFollowing)
                {
                    m_TargetLookDir = m_NavAgent.velocity;

                    m_CurrentState = States.chase;
                    //follow player
                    m_NavAgent.SetDestination(m_PlayerPos.position);
                }
                /* Are we too close to the player? */
                else if (sqrDistanceToPlayer < m_DistanceToFlee * m_DistanceToFlee)
                {
                    m_TargetLookDir = towardPlayer2D;

                    m_CurrentState = States.flee;

                    m_NavAgent.SetDestination((transform.position - m_PlayerPos.position).normalized * m_DistanceToStopFollowing);
                }
                /* We are in range of the player, not to close not too far */
                else
                {
                    m_TargetLookDir = towardPlayer2D;

                    m_CurrentState = States.strafe;

                    if (m_TimeUntilStrafeEnd > 0)
                    {
                        /* Strafe that nav meshy */
                        Vector3 strafeDir = Vector3.Cross(towardPlayer2D, Vector3.up);
                        m_NavAgent.Move((m_StrafeRight ? strafeDir : -strafeDir) * m_StrafeSpeed * Time.deltaTime);

                        m_TimeUntilStrafeEnd -= Time.deltaTime;
                    }
                    else if (m_TimeUntilStrafeStart < 0)
                    {
                        /* Initialise the strafe */
                        m_StrafeRight = Random.Range(0, 2) == 1;
                        m_TimeUntilStrafeEnd = Random.Range(m_MinStrafeTime, m_MaxStrafeTime);
                        m_TimeUntilStrafeStart = Random.Range(m_MinTimeBetweenStrafes, m_MaxTimeBetweenStrafes);
                    }
                    else
                        m_TimeUntilStrafeStart -= Time.deltaTime;

                    m_NavAgent.SetDestination(transform.position);
                }

                /* Attack */
                if (sqrDistanceToPlayer > m_MinDistanceToAttack * m_MinDistanceToAttack && sqrDistanceToPlayer < m_MaxDistanceToAttack * m_MaxDistanceToAttack)
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
                            m_CurrentState = States.attacking;
                            /* Actual attack */
                            if (m_TimeUntilNextSubAttack <= 0f)
                            {
                                Debug.Log(gameObject.name + " fired projectile");
                                /* Spawn projectile aim it at the player */
                                // NEED TO NUKE THIS WHEN POOLING IS ADDED
                                GameObject spawnedProjectile = Instantiate(m_Projectile, m_ProjectileSpawnPos.position, Quaternion.LookRotation(m_PlayerPos.position - m_ProjectileSpawnPos.position));
                                spawnedProjectile.transform.Rotate(new Vector3(Random.Range(-m_ProjectileXBloom, m_ProjectileXBloom), Random.Range(-m_ProjectileYBloom, m_ProjectileYBloom), 0f));

                                switch (m_EnemyType)
                                {
                                    case EnemyType.gruntPistol:
                                        m_Animator.SetTrigger("PistolFire");
                                        break;
                                    case EnemyType.gruntMagnum:
                                        m_Animator.SetTrigger("MagnumFire");
                                        break;
                                    case EnemyType.gruntSMG:
                                        m_Animator.SetTrigger("SMGFire");
                                        break;
                                    case EnemyType.enforcer:
                                        break;
                                    case EnemyType.rat:
                                        m_Animator.SetTrigger("Attack");
                                        break;
                                }

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
            }

            if (m_TimeUntilNextAttack > 0f)
                m_TimeUntilNextAttack -= Time.deltaTime;
        }
        
        else
        {
            m_CurrentState = States.dead;

            if (m_NavAgent.enabled)
            {
                m_ModelParent.rotation = Quaternion.LookRotation(towardPlayer2D, Vector3.up);

                if (m_WaveManager)
                    m_WaveManager.DecreaseEnemyCount();

                m_NavAgent.velocity = Vector3.zero;
                m_NavAgent.enabled = false;
            }
            lootSpawner.SpawnPickup();
            Destroy(gameObject, 5f); // NEED TO NUKE THIS FOR WHEN POOLING COMES- POOLING MAY NEVER COME :(
        }
    }

    private void LateUpdate()
    {
        ModelRotation();
    }

    /// <summary>
    /// Rotates the model to look in the target direction
    /// </summary>
    private void ModelRotation()
    {
        m_ModelParent.localPosition = Vector3.zero;
        m_ModelParent.rotation = Quaternion.Lerp(m_ModelParent.rotation, Quaternion.LookRotation(m_TargetLookDir, Vector3.up), m_ModelAllignmentSpeed);
    }

    /// <summary>
    /// Sets if the AI should be alert or not
    /// </summary>
    public void Alert(bool alert)
    {
        if (alert)
            m_CurrentState = States.chase;
        else
            m_CurrentState = States.idle;
    }
}
