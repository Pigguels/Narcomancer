using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public Key m_InteractionKey;
    public float m_MaxDistanceForInteraction = 3f;
    public float m_MaxLookAngleForInteraction = 45f;

    public enum InteractionType { OnHit, OnKeyPress, OnDistance }
    public enum ActivationType { Trigger, Toggle }
    public bool m_ActivateOnce = false;

    public InteractionType m_InteractionType;
    public ActivationType m_ActivationType;

    public bool m_Active = false;
    private bool m_HasActivated = false;

    public List<UnityEvent> m_Events;

    private Transform m_Player;
    private Transform m_PlayerHead;

    void Awake()
    {
        gameObject.tag = "Interactable";
        if (!m_Player)
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            m_PlayerHead = m_Player.Find("Head").GetComponent<Transform>();
        }
    }

    void Update()
    {
        if (m_Active)
        {
            foreach (UnityEvent uEvent in m_Events)
            {
                uEvent.Invoke();
            }

            if (m_ActivationType == ActivationType.Trigger)
                m_Active = false;
        }

        if (!(m_ActivateOnce && m_HasActivated)) // the hell..?
        {
            /* Check if the players within the interaction distance */
            if ((m_Player.position - transform.position).sqrMagnitude < (m_MaxDistanceForInteraction * m_MaxDistanceForInteraction))
            {
                /* Check if the players looking in the direction of this object */
                if (Mathf.Acos(Vector3.Dot(m_PlayerHead.forward, (transform.position - m_PlayerHead.position).normalized)) * Mathf.Rad2Deg < m_MaxLookAngleForInteraction)
                {
                    /* Check if theres nothing in the way of the player and this object */
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, (m_PlayerHead.position - transform.position).normalized, out hit, m_MaxDistanceForInteraction))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            if (m_InteractionType == InteractionType.OnDistance ||
                                (m_InteractionType == InteractionType.OnKeyPress && Keyboard.current[m_InteractionKey].isPressed))
                            {
                                if (m_ActivationType == ActivationType.Toggle)
                                    m_Active = !m_Active;
                                else
                                    m_Active = true;
                                m_HasActivated = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Interact()
    {
        if (m_InteractionType != InteractionType.OnHit || (m_ActivateOnce && m_HasActivated))
            return;

        if (m_ActivationType == ActivationType.Toggle)
            m_Active = !m_Active;
        else
            m_Active = true;
        m_HasActivated = true;
    }

    public void PrintDebugMessage(string message)
    {
        Debug.Log(name + ": " + message);
    }
}
