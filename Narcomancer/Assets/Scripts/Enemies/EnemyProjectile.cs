using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float m_MoveSpeed = 5f;
    public float m_Damage = 10f;

    public float m_TimeUntilDespawn = 5f;

    public GameObject m_ParticleEffect;

    void Update()
    {
        transform.position += transform.forward * m_MoveSpeed * Time.deltaTime;

        if (m_TimeUntilDespawn > 0f)
            m_TimeUntilDespawn -= Time.deltaTime;
        else
            Destroy(gameObject); // NEED TO NUKE THIS LINE ONCE POOLING IS IN
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " hit object: " + other.transform.name);

        /* Early out if the collider is an enemy */
        if (other.CompareTag("EnemyDamagePoint") || other.CompareTag("Enemy"))
            return;

        if (other.CompareTag("Player"))
            other.transform.GetComponent<Health>().Damage(m_Damage);

        if (m_ParticleEffect)
        {
            // NEED TO NUKE THIS LINE ONCE POOLING IS IN
            GameObject particleEffect = Instantiate(m_ParticleEffect, transform.position, transform.rotation);
        }

        // NEED TO NUKE THIS LINE ONCE POOLING IS IN
        Destroy(gameObject);
    }
}
