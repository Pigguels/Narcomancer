using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    Animator anim;

  
    

      
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoors()
    {
        print("doors are open");
        anim.SetTrigger("dooropen");
    }

    public void CloseDoors()
    {
        anim.SetTrigger("doorclose");
    }
}
