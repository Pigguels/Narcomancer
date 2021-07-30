using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    Vector2 m_MoveInput;
    Vector2 m_LookInput;

    #region Variables : Camera
    
    public float m_LookSensitivity = 4.5f;
    public float m_MaxXRot = 90;
    public float m_MinXRot = -90;
    private float m_Pitch = 0;
    private float m_Yaw = 0;
    public Transform m_Head;

    #endregion

    #region Variables : Basic Movement

    public float m_WalkSpeed = 8f;

    public float m_Gravity = 9.81f;
    public float m_Drag = 0.5f;

    private bool m_IsGrounded;

    private Vector3 m_MoveDir;
    private Vector3 m_Velocity;

    #endregion

    private CharacterController m_CharController;

    void Start()
    {
        m_CharController = GetComponent<CharacterController>();
    }

    void Update()
    {
        m_MoveDir = m_MoveInput.normalized;
        ApplyPhysics();
        m_IsGrounded = IsGrounded();

        print(m_IsGrounded);
        // do state updating here
        Walk(); // <-- garbage
    }

    private void LateUpdate()
    {
        Look();
    }

    /// <summary> 
    /// Handles the players transform and head rotation
    /// </summary>
    void Look()
    {
        /* Apply the mouse movements to the heads rotation */
        m_Pitch -= m_LookInput.y * m_LookSensitivity * Time.fixedDeltaTime;
        m_Yaw += m_LookInput.x * m_LookSensitivity * Time.fixedDeltaTime;

        /* Clamp the pitch so the head cannot look too far up or down */
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinXRot, m_MaxXRot);

        /* Apply the pitch and yaw to the transform and heads transform */
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, m_Yaw, transform.localEulerAngles.z);
        m_Head.localEulerAngles = new Vector3(m_Pitch, m_Head.localEulerAngles.y, m_Head.localEulerAngles.z);
    }


    /// <summary>
    /// Applies drag and gravity to the velocity
    /// </summary>
    private void ApplyPhysics()
    {
        /* Apply drag to the horizontal Axises */
        m_Velocity.x = Mathf.MoveTowards(m_Velocity.x, 0f, m_Drag * Time.deltaTime);
        m_Velocity.z = Mathf.MoveTowards(m_Velocity.z, 0f, m_Drag * Time.deltaTime);

        /* Apply gravity only when in the air */
        if (m_IsGrounded)
            m_Velocity.y = 0f;
        else if (m_Velocity.y > -m_Gravity)
            m_Velocity.y -= m_Gravity * Time.deltaTime;
    }

    /// <summary>
    /// Applies a force to the controllers current velocity
    /// </summary>
    public void ApplyForce(Vector3 force)
    {
        m_Velocity += force;
    }


    #region Collision Checks

    /// <summary>
    /// Returns true if the character controller is on the ground
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit sphereHit;
        return Physics.SphereCast(transform.position, m_CharController.radius, Vector3.down, out sphereHit, (m_CharController.height * 0.5f) - m_CharController.skinWidth, ~gameObject.layer);
    }

    #endregion

    #region Movement States

    /// <summary> 
    ///The players basic walk state
    /// </summary>
    private void Walk()
    {
        m_CharController.Move(((transform.forward * m_MoveDir.y + transform.right * m_MoveDir.x) * m_WalkSpeed + m_Velocity) * Time.deltaTime);
    }

    #endregion

    #region Input handling

    public void OnMove(InputAction.CallbackContext value)
    {
        m_MoveInput = value.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext value)
    {
        m_LookInput = value.ReadValue<Vector2>();
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
