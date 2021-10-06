using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
   public GameObject player;
   DoorAnimation doorAnim;
   Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Run()
    {
        //doorAnim;
       playerAnim.SetTrigger("doorkick");
       doorAnim.OpenDoors();
        
    }
}
