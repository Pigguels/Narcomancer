using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBreak : MonoBehaviour
{
    public Rigidbody[] shards;
    

    private void OnCollisionEnter(Collision collision)
    {
        
        foreach (Rigidbody rb in shards)
        {
            rb.AddForce(collision.impulse, ForceMode.Impulse);
        }
    }


}
