using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    Animator anim;

  
    

      
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoors()
    {
        print("doors are open");

        anim.SetBool("dooropen", true);
    }   

    public void CloseDoors()
    {
        print("frontdoorsclose");
        anim.SetBool("dooropen", false);
        anim.SetTrigger("doorclose");
    }
}
