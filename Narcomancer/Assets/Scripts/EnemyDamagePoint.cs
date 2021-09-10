using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagePoint : MonoBehaviour
{
    public float m_DamageMultiplier = 1f;

    public Health m_EnemyHealth;

    public void Damage(float damage)
    {
        m_EnemyHealth.Damage(damage * m_DamageMultiplier);
    }
}
