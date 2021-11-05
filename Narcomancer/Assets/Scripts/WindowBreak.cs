using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBreak : MonoBehaviour
{
    public Rigidbody[] shards;


    private void Awake()
    {
  
        foreach (Rigidbody rb in shards)
            
            rb.AddForce(Vector3.back*7.5f, ForceMode.Impulse);
        }
    }


}
