using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMovement : MonoBehaviour
{
    [Header("Rotation")]
    public float m_RotateSpeed = 0.1f;

    public float m_XLookRotationAngle;
    public float m_YLookRotationAngle;
    public float m_ZLookRotationAngle;

    [Space]
    public float m_XMoveRotationAngle;
    public float m_YMoveRotationAngle;
    public float m_ZMoveRotationAngle;

    private Quaternion m_TargetRotation;

    private Vector2 m_MoveInput;
    private Vector2 m_LookInput;

    private void Update()
    {
        // need to get the target rotation in update since math in fixed update tends to be bad
        m_TargetRotation = Quaternion.Euler(
            -Mathf.Clamp(m_MoveInput.y, -1, 1) * m_XMoveRotationAngle + -Mathf.Clamp(m_LookInput.y, -1, 1) * m_XLookRotationAngle,
            Mathf.Clamp(m_MoveInput.x, -1, 1) * m_YMoveRotationAngle + Mathf.Clamp(m_LookInput.x, -1, 1) * m_YLookRotationAngle,
            -Mathf.Clamp(m_MoveInput.x, -1, 1) * m_ZMoveRotationAngle + -Mathf.Clamp(m_LookInput.x, -1, 1) * m_ZLookRotationAngle);
    }

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TargetRotation, m_RotateSpeed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        m_LookInput = context.ReadValue<Vector2>();
    }
}
