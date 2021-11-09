using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float m_CurrentHealth = 100;
    public float m_MaxHealth = 100;

    public bool m_IsDead = false;

    public void Damage(float damage)
    {
        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            m_IsDead = true;
        }
    }

    public void Heal(float healAmount)
    {
        m_CurrentHealth += healAmount;

        if (m_CurrentHealth > m_MaxHealth)
            m_CurrentHealth = m_MaxHealth;
    }


}
