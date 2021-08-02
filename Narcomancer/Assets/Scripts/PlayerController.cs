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

    [Space]
    [Header("Camera:")]

    public float m_LookSensitivity = 4.5f;
    public float m_MaxXRot = 90;
    public float m_MinXRot = -90;
    private float m_Pitch = 0;
    private float m_Yaw = 0;
    public Transform m_Head;

    #endregion

    enum MovementStates {walk, crouch, jump, slide, mantle, vault, dash};

    #region Variables : Basic Movement

    [Space]
    [Header("Basic Movement:")]

    public float m_WalkSpeed = 8f;

    public float m_AirControl = 0.75f;

    public float m_Gravity = 9.81f;
    public float m_Drag = 0.5f;

    private bool m_IsGrounded;
    private bool m_ObjectAbove;

    private Vector3 m_MoveDir;
    private Vector3 m_Velocity;

    private MovementStates m_MoveState = MovementStates.walk;

    #endregion

    #region Variables : Jumping

    [Space]
    [Header("Jumping:")]

    public float m_JumpHeight = 2.5f;
    public float m_AirJumpMultiplier = 1f;
    public int m_MaxAirJumps = 1;

    private int m_AirJumpCount = 0;
    private Vector3 m_JumpDir;

    #endregion

    private CharacterController m_CharController;

    void Start()
    {
        m_CharController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float vel = Mathf.Sqrt(-m_JumpHeight / 1.5f * -m_Gravity);
        float t = vel / m_Gravity;
        a = vel * t + 0.5f * m_Gravity * t * t;
        print(a);
    }

    float a;

    void Update()
    {

        m_IsGrounded = IsGrounded();
        m_ObjectAbove = IsObjectAbove();

        m_MoveDir = (transform.forward * m_MoveInput.y + transform.right * m_MoveInput.x).normalized;

        /* Update the current movement states */
        switch (m_MoveState)
        {
            case MovementStates.walk:
                Walk();
                break;

            case MovementStates.crouch:
                break;

            case MovementStates.jump:
                Jump();
                break;

            case MovementStates.slide:
                break;

            case MovementStates.mantle:
                break;

            case MovementStates.vault:
                break;

            case MovementStates.dash:
                break;
        }

        ApplyPhysics();
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
        if (m_IsGrounded && m_Velocity.y < 0f)
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
        return Physics.SphereCast(transform.position, m_CharController.radius, Vector3.down, out sphereHit, (m_CharController.height * 0.5f) - m_CharController.radius + m_CharController.skinWidth + 0.01f, ~gameObject.layer);
    }

    /// <summary>
    /// Returns true if there is an object above character controller
    /// </summary>
    private bool IsObjectAbove()
    {
        RaycastHit sphereHit;
        return Physics.SphereCast(transform.position, m_CharController.radius, Vector3.up, out sphereHit, (m_CharController.height * 0.5f) - m_CharController.radius + m_CharController.skinWidth + 0.01f, ~gameObject.layer);
    }

    #endregion

    #region Movement States

    /// <summary> 
    ///The players basic walk state
    /// </summary>
    private void Walk()
    {
        if (m_IsGrounded)
            m_CharController.Move((m_MoveDir * m_WalkSpeed + m_Velocity) * Time.deltaTime);
        else                       
            m_CharController.Move((m_MoveDir * m_WalkSpeed * m_AirControl + m_Velocity) * Time.deltaTime);
    }

    private void Crouch()
    {

    }

    /// <summary>
    /// Initialises the players jump
    /// </summary>
    private void InitialiseJump()
    {
        m_AirJumpCount = 0;

        /* Apply an upward force to the velocity */
        float vel = Mathf.Sqrt(-m_JumpHeight / 2f * -m_Gravity);
        m_Velocity.y = vel * 2f;

        /* Get the initial jump direction */
        m_JumpDir = m_MoveDir;
    }
    /// <summary>
    /// The players jump, returns back to the walking state if the player has landed or hit their head on an object
    /// </summary>
    private void Jump()
    {
        /* Jump movement */
        if (m_MoveDir != Vector3.zero)
        {
            /* Move in the players desired direction */
            m_CharController.Move(((m_MoveDir * m_WalkSpeed * m_AirControl) + m_Velocity) * Time.deltaTime);

            /* Save the jump direction for the next go around */
            m_JumpDir = m_MoveDir;
        }
        else /* Move in the last desired direction */
            m_CharController.Move(((m_JumpDir * m_WalkSpeed * m_AirControl) + m_Velocity) * Time.deltaTime);


        /* Exiting the jump */
        if (m_IsGrounded && m_Velocity.y < 0f)
        {
            m_MoveState = MovementStates.walk;
        }
        else if (m_ObjectAbove)
        {
            /* Stop the upward velocity so the player doesn't hit their head */
            m_Velocity.y = 0f;

            m_MoveState = MovementStates.walk;
        }
    }

    private void Mantle()
    {

    }

    private void Vault()
    {

    }

    private void Dash()
    {

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

    public void OnCrouch()
    {

    }

    public void OnJump(InputAction.CallbackContext value)
    {

        /* On the frame it was pressed */
        if (value.ReadValueAsButton())
        {
            /* Move to the jumping state */
            if (m_IsGrounded && (m_MoveState == MovementStates.walk || m_MoveState == MovementStates.slide))
            {
                InitialiseJump();
                m_MoveState = MovementStates.jump;
            }

            /* Check if the player wants to air jump, if so apply the jump velocity */
            if (!m_IsGrounded && m_MoveState == MovementStates.jump && m_AirJumpCount < m_MaxAirJumps)
            {
                ++m_AirJumpCount;
                m_Velocity.y += Mathf.Sqrt(-m_JumpHeight / 1.5f * -m_Gravity) * m_AirJumpMultiplier;
            }
        }
    }

    public void OnDash()
    {

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
