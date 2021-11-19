using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPlayer : MonoBehaviour
{

    public GameObject player;
    public Transform resetPos;
    // Start is called before the first frame update
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.transform.position = resetPos.position;
        }
    }
}
