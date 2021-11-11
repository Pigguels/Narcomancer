using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float m_CurrentHealth = 100;
    public float m_MaxHealth = 100;

    public bool m_IsDead = false;

    public List<UnityEvent> m_OnHit;
    public List<UnityEvent> m_OnHeal;
    public List<UnityEvent> m_OnDead;

    public void Damage(float damage)
    {
        m_CurrentHealth -= damage;

        foreach (UnityEvent onHit in m_OnHit)
        {
            onHit.Invoke();
        }

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            m_IsDead = true;

            foreach (UnityEvent onDead in m_OnDead)
            {
                onDead.Invoke();
            }
        }
    }

    public void Heal(float healAmount)
    {
        m_CurrentHealth += healAmount;

        foreach (UnityEvent onHeal in m_OnHeal)
        {
            onHeal.Invoke();
        }

        if (m_CurrentHealth > m_MaxHealth)
            m_CurrentHealth = m_MaxHealth;
    }


}
