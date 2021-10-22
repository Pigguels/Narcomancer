using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCloset : MonoBehaviour
{
    private bool dooropen;  
    public GameObject door;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
       anim = door.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dooropen) { anim.Play("Door Opening"); print("dooropen"); }
        if (!dooropen) { anim.Play("Door Close"); print("doonclose"); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            dooropen = true;
        }
        else
        {
            dooropen = false;
        }
    }
 

   
}

