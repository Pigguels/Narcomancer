using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    Vector2 m_MoveInput;
    Vector2 m_LookInput;
    
    public PlayerInput m_PlayerInput;
    
    enum KeyInputs { crouch, jump, dash, primaryFire, secondaryFire, };
    bool[] m_InputDown = new bool[(int)KeyInputs.secondaryFire + 1];

    #region Variables : Camera

    [Header("Camera:")]
    [Space]

    public float m_LookSensitivity = 4.5f;
    public float m_EyeLevel = 1.9f;
    public float m_MaxXRot = 90;
    public float m_MinXRot = -90;
    private float m_Pitch = 0;
    private float m_Yaw = 0;
    public Transform m_Head;

    #endregion

    #region Variables : Basic Movement

    [Header("Basic Movement:")]
    [Space]

    public float m_WalkMoveSpeed = 8f;

    public float m_AirControl = 0.75f;

    public float m_Gravity = 9.81f;
    public float m_Drag = 0.5f;

    private bool m_IsGrounded = false;
    private bool m_ObjectAbove = false;

    private Vector3 m_MoveDir = Vector3.zero;
    private Vector3 m_LastMoveDir = Vector3.zero;
    private Vector3 m_Velocity = Vector3.zero;

    enum MovementStates {walk, crouch, jump, slide, mantle, vault, dash};
    private MovementStates m_MoveState = MovementStates.walk;

    #endregion

    #region Variables : Crouching

    [Header("Crouching:")]
    [Space]

    public float m_CrouchMoveSpeed = 4f;

    public float m_CrouchSpeed = 0.5f;

    public float m_CrouchHeight = 1f;
    private float m_StandingHeight;
    private float m_TargetHeight;

    #endregion

    #region Variables : Jumping

    [Header("Jumping:")]
    [Space]

    public float m_JumpHeight = 2.5f;
    public float m_AirJumpMultiplier = 1f;
    public int m_MaxAirJumps = 1;

    private int m_AirJumpCount = 0;
    private Vector3 m_JumpDir;

    #endregion

    #region Variables : Sliding

    public float m_AngleToStartSlide = 45f;

    #endregion

    private CharacterController m_CharController;

    private void Awake()
    {
        m_CharController = GetComponent<CharacterController>();

        m_StandingHeight = m_CharController.height;
        m_TargetHeight = m_StandingHeight;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        m_IsGrounded = IsGrounded();
        m_ObjectAbove = IsObjectAbove();

        /* Get The last move direction the is not zero */
        if (m_MoveDir != Vector3.zero)
            m_LastMoveDir = m_MoveDir;

        m_MoveDir = (transform.forward * m_MoveInput.y + transform.right * m_MoveInput.x).normalized;

        /* Update the current movement states */
        switch (m_MoveState)
        {
            case MovementStates.walk:
                Walk();
                break;

            case MovementStates.crouch:
                Crouch();

                if (!m_InputDown[(int)KeyInputs.crouch] && CanStand())
                {
                    m_TargetHeight = m_StandingHeight;
                    m_MoveState = MovementStates.walk;
                }
                break;

            case MovementStates.jump:
                Jump();
                break;

            case MovementStates.slide:
                Slide();
                break;

            case MovementStates.mantle:
                break;

            case MovementStates.vault:
                break;

            case MovementStates.dash:
                break;
        }

        ApplyPhysics();

        AdjustYScale(m_TargetHeight, m_CrouchSpeed);
    }

    private void LateUpdate()
    {
        Look();
    }

    /// <summary> 
    /// Handles the players transform and head rotation
    /// </summary>
    private void Look()
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

    #region Physics

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

    #endregion

    #region Adjustments

    /// <summary>
    /// lerp the characters y scale to the height and adjust the position to make it seamless
    /// </summary>
    private void AdjustYScale(float height, float speed)
    {
        m_CharController.enabled = false;

        /* Change the character controllers height */
        m_CharController.height = Mathf.Lerp(m_CharController.height, height, speed);

        /* Offset position */
        float yOffset = (Mathf.Lerp(m_CharController.height, height, speed) - m_CharController.height) * 0.5f;
        transform.position += new Vector3(0f, yOffset, 0f);

        /* Adjust eye level */
        m_Head.localPosition = new Vector3(m_Head.localPosition.x, m_EyeLevel * m_CharController.height * 0.5f - (1f * (m_CharController.height * 0.5f)), m_Head.localPosition.z);

        m_CharController.enabled = true;
    }

    #endregion

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

    /// <summary>
    /// Returns true if there is enough room above character controller to stand,calso returns true if already standing
    /// </summary>
    private bool CanStand()
    {
        if (m_TargetHeight == m_StandingHeight)
            return true;

        RaycastHit sphereHit;
        return !Physics.SphereCast(transform.position, m_CharController.radius, Vector3.up, out sphereHit, (m_CharController.height * 0.5f) + m_StandingHeight - m_CharController.radius + m_CharController.skinWidth + 0.01f, ~gameObject.layer);
    }

    #endregion

    #region Movement States

    /// <summary> 
    ///The players walk state
    /// </summary>
    private void Walk()
    {
        /* If the players grounded move them in their desired direction */
        if (m_IsGrounded)
            m_CharController.Move((m_MoveDir * m_WalkMoveSpeed + m_Velocity) * Time.deltaTime);
        else
        {
            /* If player tries to move move them in tht direction, if not continue in there last move direction */
            if(m_MoveDir != Vector3.zero)
                m_CharController.Move((m_MoveDir * m_WalkMoveSpeed * m_AirControl + m_Velocity) * Time.deltaTime);
            else
                m_CharController.Move((m_LastMoveDir * m_WalkMoveSpeed * m_AirControl + m_Velocity) * Time.deltaTime);
        }
    }

    /// <summary>
    /// The players crouch state
    /// </summary>
    private void Crouch()
    {
        /* If the players grounded move them in their desired direction */
        if (m_IsGrounded)
            m_CharController.Move((m_MoveDir * m_CrouchMoveSpeed + m_Velocity) * Time.deltaTime);
        else
        {
            /* If player tries to move move them in tht direction, if not continue in there last move direction */
            if (m_MoveDir != Vector3.zero)
                m_CharController.Move((m_MoveDir * m_CrouchMoveSpeed * m_AirControl + m_Velocity) * Time.deltaTime);
            else
                m_CharController.Move((m_LastMoveDir * m_CrouchMoveSpeed * m_AirControl + m_Velocity) * Time.deltaTime);
        }
    }

    /// <summary>
    /// Initialises the players jump
    /// </summary>
    private void InitialiseJump()
    {
        /* Reset the current air jumps to zero */
        m_AirJumpCount = 0;

        /* Apply an upward force to the velocity */
        float vel = Mathf.Sqrt(-m_JumpHeight / 2f * -m_Gravity) * 2f;
        m_Velocity.y = vel;

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
            m_CharController.Move(((m_MoveDir * m_WalkMoveSpeed * m_AirControl) + m_Velocity) * Time.deltaTime);

            /* Save the jump direction for the next go around */
            m_JumpDir = m_MoveDir;
        }
        else /* Move in the last desired direction */
            m_CharController.Move(((m_JumpDir * m_WalkMoveSpeed * m_AirControl) + m_Velocity) * Time.deltaTime);


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

    private void Slide()
    {

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

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        m_LookInput = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            /* Check if can transition to slide */
            if (m_IsGrounded && Vector3.Dot(m_MoveDir, transform.forward) > m_AngleToStartSlide * Mathf.Deg2Rad)
            {

            }

            /* Go to the crouched state */
            if (m_MoveState == MovementStates.walk)
            {
                m_TargetHeight = m_CrouchHeight;
                m_MoveState = MovementStates.crouch;
            }

            m_InputDown[(int)KeyInputs.crouch] = true;
        }
        else if (context.canceled)
        {
            m_InputDown[(int)KeyInputs.crouch] = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        /* On the frame it was pressed */
        if (context.started)
        {
            /* Move to the jumping state */
            if (m_IsGrounded && (m_MoveState == MovementStates.walk || m_MoveState == MovementStates.crouch || m_MoveState == MovementStates.slide))
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

            m_InputDown[(int)KeyInputs.jump] = true;
        }
        else if (context.canceled)
        {
            m_InputDown[(int)KeyInputs.jump] = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_InputDown[(int)KeyInputs.dash] = true;
        }
        else if (context.canceled)
        {
            m_InputDown[(int)KeyInputs.dash] = false;
        }
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_InputDown[(int)KeyInputs.primaryFire] = true;
        }
        else if (context.canceled)
        {
            m_InputDown[(int)KeyInputs.primaryFire] = false;
        }
    }

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_InputDown[(int)KeyInputs.secondaryFire] = true;
        }
        else if (context.canceled)
        {
            m_InputDown[(int)KeyInputs.secondaryFire] = false;
        }
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
