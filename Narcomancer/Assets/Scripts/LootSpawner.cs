using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

   public GameObject[] pickUpPrefabs;
   

    public void SpawnPickup()
    {
        for (int i = 0; i < pickUpPrefabs.Length; i++)
        {

        GameObject tempSpawn = Instantiate(pickUpPrefabs[i], new Vector3 (transform.position.x + Random.Range(-1,1), 1.5f, transform.position.z + Random.Range(-1, 1)) , pickUpPrefabs[i].transform.rotation) as GameObject;
        }

       
    }
}
