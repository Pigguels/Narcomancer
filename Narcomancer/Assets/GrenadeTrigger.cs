using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrigger : MonoBehaviour
{
    public GameObject explosion;
    public float grenadeLifetime = 3f;

  
    private void OnCollisionEnter(Collision collision)
    {
        GameObject test = Instantiate(explosion, transform.position, transform.rotation) ;
        
        Destroy(gameObject);
    }
}
