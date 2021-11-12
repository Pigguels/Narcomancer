using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    Vector2 m_MoveInput;
    Vector2 m_LookInput;

    public static bool paused = false; // jeez i hate this, but i dont have time

    public PlayerInput m_PlayerInput;
    
    enum KeyInputs { crouch, jump, dash, primaryFire, secondaryFire, };
    bool[] m_InputDown = new bool[(int)KeyInputs.secondaryFire + 1];

    public Health m_Health;

    #region Variables : Ammunition

    [Header("Ammunition:")]
    [Space]

    public int m_CurrentShotgunAmmo = 100;
    public int m_MaxShotgunAmmo = 100;
    [Space]
    public float m_CurrentNeonAmmo = 100f;
    public float m_MaxNeonAmmo = 100f;

    #endregion

    #region Variables : Camera

    [Header("Camera:")]
    [Space]

    public float m_LookSensitivity = 4.5f;
    public float m_EyeLevel = 1.9f;

    public float m_MaxXRot = 90;
    public float m_MinXRot = -90;
    [Space]
    [Range(0f, 1f)]
    public float m_CamRollSpeed = 0.3f;
    public float m_CamRollMoveInputMultiplier = 1f; // got damn these are grossly long
    public float m_CamRollLookInputMultiplier = 0.35f;
    [Space]
    public float m_CamWalkAngle = 15f;
    public float m_CamCrouchAngle = 8f;
    public float m_CamJumpAngle = 12f;
    public float m_CamSlideAngle = 25f;
    public float m_CamMantleAngle = 20f;
    public float m_CamVaultAngle = 25f;

    private float m_CamRollTargetAngle = 0f;

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

    #region Variables : Vault

    [Header("Vault:")]
    [Space]

    public float vaultTime = 1f;

    public float vaultForce = 150f;
    [Min(1.01f)]
    public float vaultClimbCurveMultiplier = 1.5f;
    public float horizontalDistanceToVault = 0.7f;
    public float verticalDistanceToVault = 0.75f;
    [Range(0f, 180f)]
    public float maxVaultWallAngle = 45f;

    private float timeSinceVaultStart = 0f;
    private float vaultTimeMultiplier;

    private Vector3 initialVaultDirection;
    private Vector3 vaultStartPosition;
    private Vector3 vaultEndPosition;
    private Vector3 vaultControlPosition;

    #endregion

    #region Variables : Dash

    [Header("Dash:")]
    [Space]

    [Range(0f, 180f)]
    public float m_DashMaxAngle = 91f;
    public float m_DashDistance = 10f;
    public float m_DashTime = 0.3f;
    public float m_DashEndForce = 2;

    public float m_DashCooldown = 1.5f;
    private float m_DashCooldownDetla;

    private Vector3 m_InitDashDir;
    private Vector3 m_DashStartPos;
    private Vector3 m_DashEndPos;
    private float m_TimeSinceDashStart = 0f;
    private float m_DashTimeMultiplier;

    private int dashLimit = 3;

    // not backward, stop gravity, continous speed, apply force on the end of the dash

    #endregion

    #region UI References

    [Header("UI Referances:")]
    [Space]

    public Slider m_HealthSlider;

    public TMP_Text m_ShotgunAmmoText;

    public Image m_NeonAmmoRing;
    public GameObject[] m_DashIcon;
    public GameObject deathScreen;

    public Animator pauseMenu;

    #endregion

    private CharacterController m_CharController;

    private LayerMask m_PlayerMask;
    private LayerMask m_IgnoreMask;


    private void Awake()
    {
        Time.timeScale = 1f;
        paused = false;
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<Animator>();
        m_CharController = GetComponent<CharacterController>();
        m_Health = GetComponent<Health>();

        m_PlayerMask = LayerMask.GetMask("Player");
        m_IgnoreMask = LayerMask.GetMask("IgnorePlayerChecks");

        m_StandingHeight = m_CharController.height;
        m_TargetHeight = m_StandingHeight;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!paused)
        {
            if (m_Health.m_IsDead == true)
            {
                PlayerDeath();
            }

            if (dashLimit != 3)
            {
                m_DashCooldownDetla += Time.deltaTime;
                if (m_DashCooldownDetla > m_DashCooldown)
                {
                    m_DashCooldownDetla = 0f;
                    dashLimit += 1;
                    DashUI();
                }
            }


            m_IsGrounded = IsGrounded();
            m_ObjectAbove = IsObjectAbove();

            /* Get The last move direction the is not zero */
            if (m_MoveDir != Vector3.zero)
                m_LastMoveDir = m_MoveDir;

            m_MoveDir = (transform.forward * m_MoveInput.y + transform.right * m_MoveInput.x).normalized;

            if ((m_MoveState == MovementStates.walk || m_MoveState == MovementStates.jump) && m_InputDown[(int)KeyInputs.jump] && m_Velocity.y >= 0f && CanVault())
            {
                m_MoveState = MovementStates.vault;
                InitialiseVault();
            }
            else if ((m_MoveState == MovementStates.walk || m_MoveState == MovementStates.jump) && m_InputDown[(int)KeyInputs.jump] && m_Velocity.y >= 0f && CanMantle())
            {
                m_MoveState = MovementStates.mantle;
                InitialiseMantle();
            }

            /* Update the current movement states */
            switch (m_MoveState)
            {
                case MovementStates.walk:
                    m_CamRollTargetAngle = m_CamWalkAngle;
                    Walk();
                    break;

                case MovementStates.crouch:
                    m_CamRollTargetAngle = m_CamCrouchAngle;
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
                    Vault();
                    break;

                case MovementStates.dash:
                    Dash();
                    break;
            }


            ApplyPhysics();

            if (m_MoveState == MovementStates.slide)
                AdjustYScale(m_TargetHeight, m_VerticalSlideSpeed);
            else
                AdjustYScale(m_TargetHeight, m_VerticalCrouchSpeed);

            AdjustCameraRoll(m_CamRollTargetAngle);

            SlopeAdjustment();

            UpdateUIElements();
        }
    }

    private void LateUpdate()
    {
        if (!paused)
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

    #region Ammunition

    /// <summary>
    /// Adds shotgun ammo to the shotgun ammo
    /// </summary>
    public void AddShotgunAmmo(int ammoAmount)
    {
        m_CurrentShotgunAmmo += ammoAmount;

        if (m_CurrentShotgunAmmo > m_MaxShotgunAmmo)
            m_CurrentShotgunAmmo = m_MaxShotgunAmmo;
    }

    /// <summary>
    /// Adds neon ammo to the neon ammo
    /// </summary>
    public void AddNeonAmmo(float ammoAmount)
    {
        m_CurrentNeonAmmo += ammoAmount;

        if (m_CurrentNeonAmmo > m_MaxNeonAmmo)
            m_CurrentNeonAmmo = m_MaxNeonAmmo;
    }

    #endregion

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
    /// Lerp the character controllers y scale to the height and adjust the position to make it seamless
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

    /// <summary>
    /// Lerp the heads z rotation to the target angle
    /// </summary>
    private void AdjustCameraRoll(float targetAngle)
    {
        /* Lerp only the z rotation of the head towards the target angle */
        m_Head.localRotation = Quaternion.Lerp(m_Head.localRotation, Quaternion.Euler(new Vector3(m_Head.localEulerAngles.x, m_Head.localEulerAngles.y, targetAngle)), m_CamRollSpeed);
    }

    /// <summary>
    /// Applies extra force to the player to account for the slope bobbing
    /// </summary>
    private void SlopeAdjustment()
    {
        /* Early out if jumping or not moving */
        if (m_MoveState == MovementStates.jump || m_MoveDir == Vector3.zero)
            return;

        /* Raycast to the ground */
        RaycastHit slopeHit;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, m_CharController.height * 0.5f + 0.5f, ~m_PlayerMask & ~m_IgnoreMask))
        {
            /* Check if the ground is a slope */
            if (Vector3.Dot(slopeHit.normal, Vector3.up) < 0.99f)
            {
                m_Velocity.y = -(m_Gravity * m_Gravity);
            }
        }
    }

    /// <summary>
    /// Updates all the UI elements
    /// eg: health text, shotgun ammo text and neon ammo text
    /// </summary>
    private void UpdateUIElements()
    {
        //m_HealthText.text = (Mathf.Round(m_Health.m_CurrentHealth * 10) / 10).ToString();
        m_HealthSlider.value = m_Health.m_CurrentHealth;
        m_HealthSlider.maxValue = m_Health.m_MaxHealth;

        m_ShotgunAmmoText.text = m_CurrentShotgunAmmo.ToString();

        //m_NeonAmmoText.text = (Mathf.Round(m_CurrentNeonAmmo * 10) / 10).ToString();
        //m_NeonAmmoSlider.value = m_CurrentNeonAmmo;
        //m_NeonAmmoSlider.maxValue = m_MaxNeonAmmo;
        m_NeonAmmoRing.fillAmount = (m_CurrentNeonAmmo / 10) * 0.25f;
    }

    #endregion

    #region Collision Checks

    /// <summary>
    /// Returns true if the character controller is on the ground
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit sphereHit;
        return Physics.SphereCast(transform.position, m_CharController.radius, Vector3.down, out sphereHit, (m_CharController.height * 0.5f) - m_CharController.radius + m_CharController.skinWidth + 0.01f, ~m_PlayerMask & ~m_IgnoreMask);
    }

    /// <summary>
    /// Returns true if there is an object above character controller
    /// </summary>
    private bool IsObjectAbove()
    {
        RaycastHit sphereHit;
        return Physics.SphereCast(transform.position, m_CharController.radius, Vector3.up, out sphereHit, (m_CharController.height * 0.5f) - m_CharController.radius + m_CharController.skinWidth + 0.01f, ~m_PlayerMask & ~m_IgnoreMask);
    }

    /// <summary>
    /// Returns true if there is enough room above character controller to stand,calso returns true if already standing
    /// </summary>
    private bool CanStand()
    {
        if (m_TargetHeight == m_StandingHeight)
            return true;

        RaycastHit sphereHit;
        return !Physics.SphereCast(transform.position, m_CharController.radius, Vector3.up, out sphereHit, (m_CharController.height * 0.5f) + (m_StandingHeight - m_CrouchHeight) - m_CharController.radius + m_CharController.skinWidth, ~m_PlayerMask & ~m_IgnoreMask);
    }

    /// <summary>
    /// Returns true if there is a mantable object in the desired direction of the player
    /// </summary>
    private bool CanMantle()
    {
        /* Early out if the players not trying to move */
        if (m_MoveDir == Vector3.zero)
            return false;

        Vector3 checkDirection = m_MoveDir;

        /* Early out if the walls angle is too high */
        if (Mathf.Acos(Vector3.Dot(checkDirection, transform.forward)) * Mathf.Rad2Deg < maxMantleWallAngle)
            return false;

        /* Check if there is a wall in the check direction */
        RaycastHit wallHit;
        if (Physics.CapsuleCast(transform.position + new Vector3(0f,m_CharController.height * 0.5f, 0f), transform.position - new Vector3(0f, m_CharController.height * 0.5f, 0f),
            m_CharController.radius, checkDirection, out wallHit, horizontalDistanceToMantle, ~m_PlayerMask & ~m_IgnoreMask))
        {
            /* Make sure the walls mantable */
            if (wallHit.transform.CompareTag("Mantlable"))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Returns true if there a vaultable object in the desired direction
    /// </summary>
    private bool CanVault()
    {
        /* Early out if the players not trying to move */
        if (m_MoveDir == Vector3.zero)
            return false;

        Vector3 checkDirection = m_MoveDir;

        /* Early out if the walls angle is too high */
        if (Mathf.Acos(Vector3.Dot(checkDirection, transform.forward)) * Mathf.Rad2Deg < maxVaultWallAngle)
            return false;

        /* Check if there is a wall in the check direction */
        RaycastHit wallHit;
        if (Physics.CapsuleCast(transform.position + new Vector3(0f, m_CharController.height * 0.5f, 0f), transform.position - new Vector3(0f, m_CharController.height * 0.5f, 0f),
            m_CharController.radius, checkDirection, out wallHit, horizontalDistanceToVault, ~m_PlayerMask & ~m_IgnoreMask))
        {
            /* Make sure the walls vaultable */
            if (wallHit.transform.CompareTag("Vaultable"))
            {
                return true;
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

        /* Update the cameras target roll angle */
        m_CamRollTargetAngle = (-m_CamWalkAngle * (m_MoveInput.x * m_CamRollMoveInputMultiplier)) + (-m_CamWalkAngle * (m_LookInput.x * m_CamRollLookInputMultiplier));
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

        /* Update the cameras target roll angle */
        m_CamRollTargetAngle = (-m_CamCrouchAngle * (m_MoveInput.x * m_CamRollMoveInputMultiplier)) + (-m_CamCrouchAngle * (m_LookInput.x * m_CamRollLookInputMultiplier));
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

        /* Update the cameras target roll angle */
        m_CamRollTargetAngle = m_CamJumpAngle;
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
            if (CanStand())
                m_TargetHeight = m_StandingHeight;
            m_MoveState = MovementStates.walk;
        }
        else if (m_ObjectAbove)
        {
            /* Stop the upward velocity so the player doesn't hit their head */
            m_Velocity.y = 0f;

            m_MoveState = MovementStates.walk;
        }

        /* Update the cameras target roll angle */
        m_CamRollTargetAngle = (-m_CamJumpAngle * (m_MoveInput.x * m_CamRollMoveInputMultiplier)) + (-m_CamJumpAngle * (m_LookInput.x * m_CamRollLookInputMultiplier));
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

        /* Reset the slides speed */
        m_SlideSpeed = m_InitSlideSpeed;

        /* Update the cameras target roll angle */
        if (m_MoveInput != Vector2.zero)
        {
            if (m_MoveInput.x > 0f)
                m_CamRollTargetAngle = -m_CamSlideAngle;
            else
                m_CamRollTargetAngle = m_CamSlideAngle;
        }
        else
        {
            if (m_LookInput.x > 0f)
                m_CamRollTargetAngle = -m_CamSlideAngle;
            else
                m_CamRollTargetAngle = m_CamSlideAngle;
        }
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
        /* Get the distance of the wall */
        RaycastHit wallHit;
        if (Physics.CapsuleCast(transform.position + new Vector3(0f, m_CharController.height * 0.5f, 0f), transform.position - new Vector3(0f, m_CharController.height * 0.5f, 0f),
            m_CharController.radius, m_MoveDir, out wallHit, horizontalDistanceToMantle, ~m_PlayerMask))
        {
            /* Try get end position of the mantle */
            RaycastHit ledgeHit;
            if (Physics.Raycast(transform.position + (m_MoveDir * (wallHit.distance + (m_CharController.radius * 2f))) + (Vector3.up * verticalDistanceToMantle),
                Vector3.down, out ledgeHit, verticalDistanceToMantle + (m_CharController.height * 0.5f), ~m_PlayerMask))
            {
                /* Get the start and end positions of the mantle */
                mantleStartPosition = transform.position;
                mantleEndPosition = ledgeHit.point + new Vector3(0f, m_CharController.height * 0.5f + m_CharController.skinWidth, 0f);

                /* Get the control point for the mantles bezier curve animation */
                mantleControlPosition = new Vector3(mantleStartPosition.x, ledgeHit.point.y + (m_CharController.height * 0.5f), mantleStartPosition.z);
                Vector3 controlPointDirection = ((mantleControlPosition - mantleStartPosition).normalized + (mantleControlPosition - mantleEndPosition).normalized) * 0.5f;
                mantleControlPosition += (controlPointDirection * mantleClimbCurveMultiplier);
            }
            else
            /* Early out of the mantle if for some reason theres no ledge */
            {
                Debug.Log("Early outed mantle, no ledge found");
                m_MoveState = MovementStates.walk;
                return;
            }
        }
        else /* Early out of the mantle if for some reason theres no ledge */
        {
            Debug.Log("Early outed mantle, no wall found");
            m_MoveState = MovementStates.walk;
            return;
        }

        // get the mantle time multiplier
        mantleTimeMultiplier = 1 / mantleTime;

        // reset the mantle timer
        timeSinceMantleStart = 0f;

        /* Update the cameras target roll angle */
        if (m_MoveInput != Vector2.zero)
        {
            if (m_MoveInput.x > 0f)
                m_CamRollTargetAngle = -m_CamMantleAngle;
            else
                m_CamRollTargetAngle = m_CamMantleAngle;
        }
        else
        {
            if (m_LookInput.x > 0f)
                m_CamRollTargetAngle = -m_CamMantleAngle;
            else
                m_CamRollTargetAngle = m_CamMantleAngle;
        }
    }
    /// <summary>
    /// The players mantle, returns back to the walk state once completed
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

    /// <summary>
    /// Initialises the players slide
    /// </summary>
    private void InitialiseVault()
    {
        /* Get the distance of the wall */
        RaycastHit wallHit;
        if (Physics.CapsuleCast(transform.position + new Vector3(0f, m_CharController.height * 0.5f, 0f), transform.position - new Vector3(0f, m_CharController.height * 0.5f, 0f),
            m_CharController.radius, m_MoveDir, out wallHit, horizontalDistanceToVault, ~m_PlayerMask))
        {
            /* Get end position of the vault */
            RaycastHit ledgeHit;
            if (Physics.Raycast(transform.position + (m_MoveDir * (wallHit.distance + (m_CharController.radius * 2f))) + (Vector3.up * verticalDistanceToVault),
                Vector3.down, out ledgeHit, verticalDistanceToVault + (m_CharController.height * 0.5f), ~m_PlayerMask))
            {
                /* Get the start and end positions of the mantle */
                vaultStartPosition = transform.position;
                vaultEndPosition = ledgeHit.point + new Vector3(0f, m_CharController.height * 0.5f + m_CharController.skinWidth, 0f);

                /* Get the control point for the mantles bezier curve animation */
                vaultControlPosition = new Vector3(vaultStartPosition.x, ledgeHit.point.y + (m_CharController.height * 0.5f), vaultStartPosition.z);
                Vector3 controlPointDirection = ((vaultControlPosition - vaultStartPosition).normalized + (vaultControlPosition - vaultEndPosition).normalized) * 0.5f;
                vaultControlPosition += (controlPointDirection * vaultClimbCurveMultiplier);
            }
            else 
            /* Early out of the vault if for some reason theres no ledge */
            {
                m_MoveState = MovementStates.walk;
                return;
            }
        }
        else
        /* Early out of the vault if for some reason theres no ledge */
        {
            m_MoveState = MovementStates.walk;
            return;
        }

        // get the initial vault direction
        initialVaultDirection = m_MoveDir;

        // get the vault time multiplier
        vaultTimeMultiplier = 1 / vaultTime;

        // reset the vault timer
        timeSinceVaultStart = 0f;

        /* Update the cameras target roll angle */
        if (m_MoveInput != Vector2.zero)
        {
            if (m_MoveInput.x > 0f)
                m_CamRollTargetAngle = -m_CamVaultAngle;
            else
                m_CamRollTargetAngle = m_CamVaultAngle;
        }
        else
        {
            if (m_LookInput.x > 0f)
                m_CamRollTargetAngle = -m_CamVaultAngle;
            else
                m_CamRollTargetAngle = m_CamVaultAngle;
        }
    }
    /// <summary>
    /// The players vault, returns back to the walk state once completed
    /// </summary>
    private void Vault()
    {
        timeSinceVaultStart += Time.deltaTime;

        if (timeSinceVaultStart * vaultTimeMultiplier < 1f)
        {
            /* lerp along a bezier curve towards the target point */
            transform.position = QuadraticBezier(vaultStartPosition, vaultEndPosition, vaultControlPosition, timeSinceVaultStart * vaultTimeMultiplier);
        }
        else
        {
            /*apply force, exit the vault and return back to walking*/
            ApplyForce(initialVaultDirection * vaultForce);

            m_MoveState = MovementStates.walk;
        }
    }

    /// <summary>
    /// The players dash, returns back to the walk state once completed
    /// </summary>
    private void Dash()
    {
        m_TimeSinceDashStart += Time.deltaTime;

        if (m_TimeSinceDashStart * m_DashTimeMultiplier < 1f)
        {
            /* Check if the dash is blocked */
            RaycastHit wallHit;
            if (Physics.CapsuleCast(transform.position + new Vector3(0f, m_CharController.height * 0.5f, 0f), transform.position - new Vector3(0f, m_CharController.height * 0.5f, 0f),
                m_CharController.radius, m_MoveDir, out wallHit, m_DashDistance * (Time.deltaTime * m_DashTimeMultiplier) + m_CharController.radius, ~m_PlayerMask))
            {
                /* End the dash */
                m_MoveState = MovementStates.walk;
                return;
            }

            /* Do the dash */
            m_Velocity = Vector3.zero;
            transform.position = Vector3.Lerp(m_DashStartPos, m_DashEndPos, m_TimeSinceDashStart * m_DashTimeMultiplier);


        }
        else
        {
            /* Apply a force in the direction of the dash */
            ApplyForce(m_InitDashDir * m_DashEndForce);

            m_MoveState = MovementStates.walk;
        }
    }

    private void DashUI()
    {
        if (dashLimit == 3)
        {
            m_DashIcon[0].SetActive(true);
            m_DashIcon[1].SetActive(true);
            m_DashIcon[2].SetActive(true);
        }
        if (dashLimit == 2)
        {
            m_DashIcon[0].SetActive(true);
            m_DashIcon[1].SetActive(true);
            m_DashIcon[2].SetActive(false);
        }
        if (dashLimit == 1)
        {
            m_DashIcon[0].SetActive(true);
            m_DashIcon[1].SetActive(false);
            m_DashIcon[2].SetActive(false);
        }
        if (dashLimit == 0)
        {
            m_DashIcon[0].SetActive(false);
            m_DashIcon[1].SetActive(false);
            m_DashIcon[2].SetActive(false);
        }

    }

    #endregion

    #region Input handling

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            m_MoveInput = context.ReadValue<Vector2>();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            m_LookInput = context.ReadValue<Vector2>();
        }
    }
    
    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            if (context.started && !paused)
            {
                m_InputDown[(int)KeyInputs.primaryFire] = true;
            }
            else if (context.canceled && !paused)
            {
                m_InputDown[(int)KeyInputs.primaryFire] = false;
            }
        }
    }

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            if (context.started && !paused)
            {
                m_InputDown[(int)KeyInputs.secondaryFire] = true;
            }
            else if (context.canceled && !paused)
            {
                m_InputDown[(int)KeyInputs.secondaryFire] = false;
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            if (context.started && !paused)
            {
                /* Check if can transition to slide */
                if (m_IsGrounded && Mathf.Acos(Vector3.Dot(m_MoveDir, transform.forward)) * Mathf.Rad2Deg < m_MaxAngleToStartSlide)
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
            else if (context.canceled && !paused)
            {
                m_InputDown[(int)KeyInputs.crouch] = false;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            /* On the frame it was pressed */
            if (context.started && !paused)
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
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            if (context.started && !paused)
            {
                /* Try go into dash */
                if ((m_MoveState == MovementStates.walk || m_MoveState == MovementStates.jump || m_MoveState == MovementStates.dash) && dashLimit != 0)
                {
                    /* Make sure the move directions angle isn't too high */
                    if (m_MoveDir != Vector3.zero && Mathf.Acos(Vector3.Dot(m_MoveDir, transform.forward)) * Mathf.Rad2Deg < m_DashMaxAngle)
                    {
                        /* Initialise the dash */
                        m_DashStartPos = transform.position;
                        m_DashEndPos = transform.position + (m_MoveDir * m_DashDistance);
                        m_InitDashDir = m_MoveDir;
                        m_TimeSinceDashStart = 0f;
                        m_DashTimeMultiplier = 1f / m_DashTime;

                        m_Velocity = Vector3.zero;

                        m_MoveState = MovementStates.dash;

                        dashLimit -= 1;
                        m_DashCooldownDetla = 0f;
                        DashUI();
                    }
                }

                m_InputDown[(int)KeyInputs.dash] = true;
            }
            else if (context.canceled && !paused)
            {
                m_InputDown[(int)KeyInputs.dash] = false;
            }
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        
        if (context.started)
        {
            if (!m_Health.m_IsDead)
            {
                if (!paused)
                {
                    pauseMenu.SetBool("Paused", true);
                    Time.timeScale = 0f;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    paused = true;
                }
                else
                {
                    pauseMenu.SetBool("Paused", false);
                    Time.timeScale = 1f;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    paused = false;
                }
            }
            else

            {
                /* Return to menu if pause is pressed during death screen */
                Debug.Log("Loading Main Menu...");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void OnPauseUI()
    {
        if (!paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetBool("Paused", true);
            Debug.Log("You did it buddy!");
            Time.timeScale = 0f;
            paused = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetBool("Paused", false);
            Debug.Log("You did it buddy!");
            Time.timeScale = 1f;
            paused = false;
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

    public void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("The player has died");
        deathScreen.SetActive(true);
        Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, 0.25f);
    }
}
