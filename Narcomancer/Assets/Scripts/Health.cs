using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float m_Health = 100;
    public float m_MaxHealth = 100;

    public bool m_IsDead = false;

    public void Damage(float damage)
    {
        m_Health -= damage;

        if (m_Health <= 0)
        {
            m_Health = 0;
            m_IsDead = true;
        }
    }

    public void Heal(float healAmount)
    {
        m_Health += healAmount;

        if (m_Health > m_MaxHealth)
            m_Health = m_MaxHealth;
    }
}
