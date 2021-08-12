using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerGlove : MonoBehaviour
{
    public int maxChainAmount = 4;
    public float distanceToChain = 4f;
    public float damagePerSecond = 60f;
    public float range = 20;

    [Space]
    [Header("References")]
    public Transform playerCamera;

    private List<GameObject> hitObjects;

    private LayerMask playerLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");

        hitObjects = new List<GameObject>();
    }

    void ShootLightning()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, ~playerLayer))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hitObjects.Add(hit.transform.gameObject);

                // chain to nearby enemies
                for (int i = 0; i < maxChainAmount + 1; ++i)
                {
                    GameObject closestEnemy = GetClosestEnemyToEnemy(hitObjects[hitObjects.Count - 1]);

                    // if there is no other enemy
                    if (!closestEnemy)
                        break;

                    if (Vector3.SqrMagnitude(closestEnemy.transform.position - hitObjects[hitObjects.Count - 1].transform.position) <= (distanceToChain * distanceToChain))
                        hitObjects.Add(closestEnemy);
                }

                // damage the hit enemies
                foreach (GameObject enemyToDamage in hitObjects)
                {
                    //enemyToDamage.GetComponent<Enemy>().TakeDamage(pelletDamage);

                    enemyToDamage.GetComponent<DestructibleObject>().TakeDamage(damagePerSecond * Time.deltaTime);
                }

                //// SPAWN DEBUG CUBES
                //for (int i = 1; i < hitObjects.Count - 1; ++i)
                //{
                //    Instantiate(testcube, hitObjects[i - 1].transform.position + (hitObjects[i].transform.position - hitObjects[i - 1].transform.position), Quaternion.identity);
                //}
            }
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
            foreach (GameObject chainedEnemy in hitObjects)
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

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            hitObjects.Clear();
            ShootLightning();
        }
    }
}
