using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

   public GameObject pickUpPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPickup()
    {
        GameObject tempSpawn = Instantiate(pickUpPrefab, new Vector3 (transform.position.x, 1.5f, transform.position.z) , pickUpPrefab.transform.rotation) as GameObject;

       /* tempSpawn.transform.position = transform.position;*/


       // tempSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
    }
}
