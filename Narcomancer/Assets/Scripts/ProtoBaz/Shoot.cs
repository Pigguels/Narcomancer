using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{

    public Animator anim;


    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current.leftAltKey.isPressed)
        {
            anim.SetTrigger("Shoot");
        }
    }
}
