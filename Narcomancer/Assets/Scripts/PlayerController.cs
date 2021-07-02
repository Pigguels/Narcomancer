using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Vector2 lookInput;

    void Start()
    {
        
    }

    void Update()
    {

    }

    #region Input handling

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        moveInput = input;
    }
    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        lookInput = input;
    }

    #endregion


    #region Device connections
    public void OnDeviceLost()
    {

    }
    public void OnDeviceRegained()
    {

    }
    public void OnControlsChanged()
    {

    }
    #endregion
}
