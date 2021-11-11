using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

    public GameObject[] pickUpPrefabs;
    PlayerController playerController;
    bool spawned =false;
    private void Start()
    {
       playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SpawnPickup()
    {
        if (!spawned)
        {
            if (playerController.m_CurrentShotgunAmmo < playerController.m_MaxShotgunAmmo *0.75 )
            {
                GameObject tempSpawn = Instantiate(pickUpPrefabs[0], new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + 1f, transform.position.z + Random.Range(-1, 1)), pickUpPrefabs[0].transform.rotation);
            } 
            if (playerController.m_CurrentNeonAmmo < playerController.m_MaxNeonAmmo * 0.75)
            {
                GameObject tempSpawn = Instantiate(pickUpPrefabs[2], new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + 1f, transform.position.z + Random.Range(-1, 1)), pickUpPrefabs[2].transform.rotation);
            }  
            if (playerController.m_Health.m_CurrentHealth < playerController.m_Health.m_MaxHealth * 0.75)
            {
                GameObject tempSpawn = Instantiate(pickUpPrefabs[1], new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + 1f, transform.position.z + Random.Range(-1, 1)), pickUpPrefabs[1].transform.rotation);
            }

            spawned = true;
        }
    }
}
