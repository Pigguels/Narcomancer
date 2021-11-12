using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrigger : MonoBehaviour
{
    public GameObject explosion;
    public float grenadeLifetime = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "IgnoreGrenade" || collision.gameObject.name == "Narcomancer_v03")
            return;
        else
        {
            GameObject test = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(test, grenadeLifetime);
            Destroy(gameObject);
        }
        
    }
}
