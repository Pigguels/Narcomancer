using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PlayerPowerGlove : MonoBehaviour
{
    public int m_MaxChainAmount = 4;
    public float m_DistanceToChain = 4f;
    public float m_DamagePerSecond = 60f;
    public float m_Range = 20;

    [Space]
    [Header("References")]
    public Transform m_PlayerCamera;
    public Transform m_HandPosition;

    private PlayerController m_PlayerController;

    private LineRenderer m_LineRenderer;

    private List<GameObject> m_HitObjects;

    private LayerMask m_PlayerLayerMask;

    private bool m_SecondaryFireDown = false;

    public Animator fpRig;

    void Start()
    {
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        m_LineRenderer = GetComponent<LineRenderer>();

        m_PlayerLayerMask = LayerMask.GetMask("Player");

        m_HitObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (!PlayerController.paused)
        {
            if (m_SecondaryFireDown && m_PlayerController.m_CurrentNeonAmmo > 0f)
            {
                ShootLightning();
                m_PlayerController.m_CurrentNeonAmmo -= Time.deltaTime;
                fpRig.SetBool("PlayerLightning", true);
            }
            else
            {
                m_LineRenderer.positionCount = 0;
                fpRig.SetBool("PlayerLightning", false);
            }
        }
    }

    void ShootLightning()
    {
        /* Reset the hit object list */
        m_HitObjects.Clear();

        /* Reset the linerenderers positions */
        m_LineRenderer.positionCount = m_MaxChainAmount + 1;
        m_LineRenderer.SetPosition(0, m_HandPosition.position);

        RaycastHit hit;
        if (Physics.Raycast(m_PlayerCamera.position, m_PlayerCamera.forward, out hit, m_Range, ~m_PlayerLayerMask))
        {

            if (hit.transform.CompareTag("Interactable"))
            {
                m_HitObjects.Add(hit.transform.gameObject);
                if (hit.transform.GetComponent<Health>())
                    hit.transform.GetComponent<Health>().Damage(m_DamagePerSecond * Time.deltaTime);
                // NEED TO ADD GENERIC INTERACTABLE OBJECT EVENT CALLING HERE
            }
            else if (hit.transform.CompareTag("EnemyDamagePoint"))
            {
                m_HitObjects.Add(hit.transform.GetComponent<EnemyDamagePoint>().m_EnemyHealth.gameObject);

                // chain to nearby enemies
                for (int i = 0; i < m_MaxChainAmount + 1; ++i)
                {
                    GameObject closestEnemy = GetClosestEnemyToEnemy(m_HitObjects[m_HitObjects.Count - 1]);

                    // if there is no other enemy
                    if (!closestEnemy)
                        break;

                    if (Vector3.SqrMagnitude(closestEnemy.transform.position - m_HitObjects[m_HitObjects.Count - 1].transform.position) <= (m_DistanceToChain * m_DistanceToChain))
                        m_HitObjects.Add(closestEnemy);
                }

                /* Go through and add each hit objects position to the linerenderers vertex list */
                for (int i = 1; i < m_MaxChainAmount + 1; ++i)
                {
                    if (i < m_HitObjects.Count)
                        m_LineRenderer.SetPosition(i, m_HitObjects[i - 1].transform.position + m_HitObjects[i - 1].GetComponent<EnemyAI>().m_LightningChainOffset);
                    else
                        m_LineRenderer.SetPosition(i, m_LineRenderer.GetPosition(i - 1));
                }

                // damage the hit enemies
                foreach (GameObject enemyToDamage in m_HitObjects)
                {
                    if (enemyToDamage.GetComponent<Health>())
                        enemyToDamage.GetComponent<Health>().Damage(m_DamagePerSecond * Time.deltaTime);
                }
            }
            else
            {
                /* If the lighting didn't hit any object, add the hit point the the vertex list */
                m_LineRenderer.SetPosition(1, hit.point);
                m_LineRenderer.positionCount = 2;
            }
        }
        else
        {
            /* If the lighting didn't hit any object, add the hit point the the vertex list */
            m_LineRenderer.SetPosition(1, m_PlayerCamera.position + (m_PlayerCamera.forward * m_Range));
            m_LineRenderer.positionCount = 2;
        }
    }

    /// <summary>
    /// returns the enemy that is closest to the inputed enemy - returns null if there is no other enemies or it is itself
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    GameObject GetClosestEnemyToEnemy(GameObject enemy)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = enemies[0];

        // make sure the closestEnemy's first assignment is not itself
        if (closestEnemy == enemy && enemies.Length > 1)
            closestEnemy = enemies[1];

        foreach (GameObject enemyToCheck in enemies)
        {
            // make sure the enemy isn't the original enemy - if so skip the enemy
            if (enemyToCheck == enemy)
                continue;

            // make sure the enemy hasn't been chained already - if so skip them
            bool enemyIsChained = false;
            foreach (GameObject chainedEnemy in m_HitObjects)
            {
                if (enemyToCheck == chainedEnemy)
                    enemyIsChained = true;
            }
            if (enemyIsChained)
                continue;

            // if the enemyToCheck is closer to the original enemy than the previous closestEnemy, make them the new closestEnemy
            if (Vector3.SqrMagnitude(enemy.transform.position - enemyToCheck.transform.position) < Vector3.SqrMagnitude(enemy.transform.position - closestEnemy.transform.position))
                closestEnemy = enemyToCheck;
        }

        // return null if the closestEnemy is the original enemy
        if (closestEnemy == enemy)
            return null;

        return closestEnemy;
    }

    //public void OnSecondaryFire(InputAction.CallbackContext context)
    //{
    //    if (m_PlayerController.paused == false)
    //    {
    //        if (context.started)
    //            m_SecondaryFireDown = true;
    //        else if (context.canceled)
    //            m_SecondaryFireDown = false;
    //    }
    //}
}
