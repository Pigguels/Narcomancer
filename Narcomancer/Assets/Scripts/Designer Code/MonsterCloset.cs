using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCloset : MonoBehaviour
{
    public bool dooropen;
    public GameObject door;
    Animator anim;
    public int counter;
    // Start is called before the first frame update
    void Start()
    {
       anim = door.GetComponent<Animator>();
        counter = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 0) { dooropen = false; }
        if (counter >0) { dooropen = true; }
        if (dooropen) { anim.SetBool("dooropen", true); }//print("dooropen"); }
        if (!dooropen) { anim.SetBool("dooropen", false); }// print("doonclose"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            counter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            counter--;
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //   if (other.tag == "Player")
    // {
    //   nobodyhome = false;
    // dooropen = true;

    // }


    //private void OnTriggerExit(Collider other)
    //{
    //  if (other.tag == "Player")
    // {
    //    nobodyhome = true;
    //  print("playerleft");
    //  }
    //}


}

