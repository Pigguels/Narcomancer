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

    [Range(0f, 1f)]
    public float m_VerticalCrouchSpeed = 0.5f;
    public float m_CrouchHeight = 1f;

    public float m_CrouchMoveSpeed = 4f;

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

    [Header("Slide Variables:")]
    [Space]

    [Range(0f, 1f)]
    public float m_VerticalSlideSpeed = 0.7f;
    public float m_SlideHeight = 1f;

    [Range(0f, 180f)]
    public float m_MaxAngleToStartSlide = 45f;
    public float m_InitSlideSpeed = 45f;
    public float m_SlideFriction = 4f;

    private float m_SlideSpeed;
    private Vector3 m_InitSlideDir;

    #endregion

    #region Variables : Mantle

    [Header("Mantle:")]
    [Space]

    public float mantleTime = 1f;
    [Min(1.01f)]
    public float mantleClimbCurveMultiplier = 1.2f;
    public float horizontalDistanceToMantle = 0.1f;
    public float verticalDistanceToMantle = 1.5f;
    [Range(0f, 180f)]
    public float maxMantleWallAngle = 45f;

    private float timeSinceMantleStart = 0f;
    private float mantleTimeMultiplier;

    private Vector3 mantleStartPosition;
    private Vector3 mantleEndPosition;
    private Vector3 mantleControlPosition;

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

        print(CanMantle());
        if (m_InputDown[(int)KeyInputs.jump] && m_Velocity.y >= 0f && CanMantle())
        {
            InitialiseMantle();
            m_MoveState = MovementStates.mantle;
        }

        /* Update the current movement states */
        switch (m_MoveState)
        {
            case MovementStates.walk:
                Walk();
                break;

            case MovementStates.crouch:
                Crouch();
                break;

            case MovementStates.jump:
                Jump();
                break;

            case MovementStates.slide:
                Slide();
                break;

            case MovementStates.mantle:
                Mantle();
                break;

            case MovementStates.vault:
                break;

            case MovementStates.dash:
                break;
        }

        ApplyPhysics();

        if (m_MoveState == MovementStates.slide)
            AdjustYScale(m_TargetHeight, m_VerticalSlideSpeed);
        else
            AdjustYScale(m_TargetHeight, m_VerticalCrouchSpeed);
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

        float controllerHeightCache = m_CharController.height;

        /* Change the character controllers height */
        m_CharController.height = Mathf.Lerp(controllerHeightCache, height, speed);

        /* Offset position */
        float yOffset = (Mathf.Lerp(controllerHeightCache, height, speed) - controllerHeightCache) * 0.5f;
        transform.position += new Vector3(0f, yOffset, 0f);

        /* Adjust eye level */
        m_Head.localPosition = new Vector3(m_Head.localPosition.x, m_EyeLevel * m_CharController.height * 0.5f - (m_CharController.height * 0.5f), m_Head.localPosition.z);
        
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

    private bool CanMantle()
    {
        //Vector3 checkDirection = m_MoveDir != Vector3.zero ? m_MoveDir : transform.forward;
        ///*        
        //         ___
        //        /***\
        //        |***|________\  |
        //        |***|        /  |
        //        |***|           |
        //        /***\________\  |
        //        |***|        /  v
        //        \___/   ________X____
        //        |   |-->x
        //        |   |   |
        //        |   |   |
        //        \___/   |
        // */
        //
        ///* Check if there is a wall in the check direction */
        //RaycastHit wallHit;
        //if (Physics.Raycast(transform.position + new Vector3(0f, 0f, 0f), checkDirection, out wallHit, m_CharController.radius + m_CharController.skinWidth + horizontalDistanceToMantle, ~gameObject.layer))
        //{
        //    /* Make sure the angle of the wall is not to high */
        //    if (Vector3.Dot(wallHit.normal, transform.forward) >= maxMantleWallAngle * Mathf.Deg2Rad)
        //    {
        //        /* Make sure there is actually a ledge, and not a wall */
        //        RaycastHit ledgeHit;
        //        if (!Physics.Raycast(transform.position + new Vector3(0f, verticalDistanceToMantle, 0f), checkDirection, out ledgeHit, m_CharController.radius + m_CharController.skinWidth + horizontalDistanceToMantle, ~gameObject.layer))
        //        {
        //            /* Make sure there is a surface to land on */
        //            RaycastHit groundHit;
        //            if (!Physics.Raycast(transform.position + new Vector3(0f, verticalDistanceToMantle, 0f), Vector3.down, out groundHit, verticalDistanceToMantle, ~gameObject.layer))
        //            {
        //                /* Make sure the surface if fairly level */
        //                if (Vector3.Dot(groundHit.normal, transform.up) >= 0.95f)
        //                {
        //                    RaycastHit playerCollisionHit;
        //                    if (!Physics.CapsuleCast(transform.position + new Vector3(0f, verticalDistanceToMantle - groundHit.distance - m_CharController.radius + m_CharController.skinWidth, 0f),
        //                        transform.position + new Vector3(0f, (verticalDistanceToMantle - groundHit.distance) + m_CharController.height + m_CharController.skinWidth, 0f), m_CharController.radius, -wallHit.normal, horizontalDistanceToMantle, ~gameObject.layer))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        Vector3 checkDirection = m_MoveDir != Vector3.zero ? m_MoveDir : transform.forward;

        Debug.DrawLine(transform.position, transform.position + checkDirection * (m_CharController.radius + m_CharController.skinWidth + horizontalDistanceToMantle), Color.green);
        Debug.DrawLine(transform.position + (checkDirection * (horizontalDistanceToMantle + (m_CharController.radius * 2f))) + (Vector3.up * verticalDistanceToMantle), transform.position + (checkDirection * (horizontalDistanceToMantle + (m_CharController.radius * 2f))), Color.magenta);


        // check if there's a wall in the direction the players moving
        RaycastHit wallHit;
        if (Physics.Raycast(transform.position, checkDirection, out wallHit, m_CharController.radius + m_CharController.skinWidth + horizontalDistanceToMantle, ~gameObject.layer))
        {
            // check if the angle of the wall is within the max mantle angle
            if (Vector3.Dot(wallHit.normal, checkDirection) < -maxMantleWallAngle * Mathf.Deg2Rad)
            {
                // check if there's a surface above that wall
                RaycastHit groundHit;
                if (Physics.Raycast(transform.position + (checkDirection * (horizontalDistanceToMantle + (m_CharController.radius * 2f))) + (Vector3.up * verticalDistanceToMantle), Vector3.down, out groundHit, verticalDistanceToMantle, ~gameObject.layer))
                {
                    // check if the ledges surface is close to level
                    if (Vector3.Dot(groundHit.normal, Vector3.up) > 0.9f)
                    {
                        if (!Physics.CapsuleCast(transform.position + new Vector3(0f, verticalDistanceToMantle - groundHit.distance + m_CharController.radius + m_CharController.skinWidth, 0f),
                            transform.position + new Vector3(0f, (verticalDistanceToMantle - groundHit.distance) + m_CharController.height + m_CharController.radius + m_CharController.skinWidth, 0f), m_CharController.radius, -wallHit.normal, horizontalDistanceToMantle, ~gameObject.layer) &&
                            !Physics.CapsuleCast(transform.position + new Vector3(0f, (verticalDistanceToMantle - groundHit.distance) + m_CharController.height + m_CharController.radius + m_CharController.skinWidth, 0f),
                            transform.position + new Vector3(0f, verticalDistanceToMantle - groundHit.distance + m_CharController.radius + m_CharController.skinWidth, 0f), m_CharController.radius, -wallHit.normal, horizontalDistanceToMantle, ~gameObject.layer))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
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
        /* Exit the crouch state */
        if (!m_InputDown[(int)KeyInputs.crouch] && CanStand())
        {
            m_TargetHeight = m_StandingHeight;
            m_MoveState = MovementStates.walk;
            return;
        }

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

    /// <summary>
    /// Initialises the players slide
    /// </summary>
    private void InitialiseSlide()
    {
        /* Set new target height */
        m_TargetHeight = m_SlideHeight;

        /* Get the initial slide direction */
        m_InitSlideDir = m_MoveDir;

        m_SlideSpeed = m_InitSlideSpeed;
    }
    /// <summary>
    /// the players slide, returns back to the crouched state once completed
    /// </summary>
    private void Slide()
    {
        /* Slide movement */
        if (m_SlideSpeed > m_CrouchMoveSpeed)
        {
            /* Create the slide movement vector */
            Vector3 movement = (m_InitSlideDir + (m_MoveDir * 0.5f)).normalized;

            /* Apply the slide movement */
            if (m_IsGrounded)
                m_CharController.Move(((movement * m_SlideSpeed) + m_Velocity) * Time.deltaTime);
            else
                m_CharController.Move(((movement * m_SlideSpeed * m_AirControl) + m_Velocity) * Time.deltaTime);

            /* Slow the slide speed */
            m_SlideSpeed = Mathf.Lerp(m_SlideSpeed, 0f, m_SlideFriction * Time.deltaTime);
        }
        else
        {
            /* Exit the slide */
            m_TargetHeight = m_CrouchHeight;
            m_MoveState = MovementStates.crouch;
        }
    }

    /// <summary>
    /// Initialises the players mantle
    /// </summary>
    private void InitialiseMantle()
    {
        // try get the mantles target position
        Vector3 checkDirection = m_MoveDir != Vector3.zero ? m_MoveDir : transform.forward;
        RaycastHit ledgeHit;
        if (Physics.Raycast(transform.position + (checkDirection * (horizontalDistanceToMantle + (m_CharController.radius * 2f))) + (Vector3.up * verticalDistanceToMantle), Vector3.down, out ledgeHit, verticalDistanceToMantle, ~gameObject.layer))
        {
            // get the start and end positions of the mantle
            mantleStartPosition = transform.position;
            mantleEndPosition = ledgeHit.point + (Vector3.up * (m_CharController.height * 0.5f));

            // get the control point for the mantles bezier curve animation
            mantleControlPosition = new Vector3(mantleStartPosition.x, ledgeHit.point.y + (transform.localScale.y * (m_CharController.height * 0.5f)), mantleStartPosition.z);
            Vector3 controlPointDirection = ((mantleControlPosition - mantleStartPosition).normalized + (mantleControlPosition - mantleEndPosition).normalized) * 0.5f;
            mantleControlPosition += (controlPointDirection * mantleClimbCurveMultiplier);
        }
        else
        {
            // early out the mantle if for some reason there's no more ledge
            m_MoveState = MovementStates.walk;
            return;
        }

        // get the mantle time multiplier
        mantleTimeMultiplier = 1 / mantleTime;

        // reset the mantle timer
        timeSinceMantleStart = 0f;
    }
    /// <summary>
    /// the players mantle, returns back to the walk state once completed
    /// </summary>
    private void Mantle()
    {
        timeSinceMantleStart += Time.deltaTime;

        if (timeSinceMantleStart * mantleTimeMultiplier < 1f)
        { 
            /* Lerp along a bezier curve towards the target point */
            transform.position = QuadraticBezier(mantleStartPosition, mantleEndPosition, mantleControlPosition, timeSinceMantleStart * mantleTimeMultiplier);
        }
        else
        {
            /* Exit the mantle and return back to walking */
            m_MoveState = MovementStates.walk;
        }
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

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            /* Check if can transition to slide */
            if (m_IsGrounded && Vector3.Dot(m_MoveDir, transform.forward) > m_MaxAngleToStartSlide * Mathf.Deg2Rad)
            {
                InitialiseSlide();
                m_MoveState = MovementStates.slide;
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


    Vector3 QuadraticBezier(Vector3 startPos, Vector3 endPos, Vector3 controlPos, float t)
    {
        return new Vector3(Mathf.Pow(1 - t, 2) * startPos.x +
            (1 - t) * 2 * t * controlPos.x +
            t * t * endPos.x,
        Mathf.Pow(1 - t, 2) * startPos.y +
            (1 - t) * 2 * t * controlPos.y +
            t * t * endPos.y,
        Mathf.Pow(1 - t, 2) * startPos.z +
            (1 - t) * 2 * t * controlPos.z +
            t * t * endPos.z);
    }
}
