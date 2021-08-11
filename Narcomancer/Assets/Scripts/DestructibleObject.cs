using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float health = 10f;
    public float maxHealth = 10f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
            Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
            health = maxHealth;
    }
}
