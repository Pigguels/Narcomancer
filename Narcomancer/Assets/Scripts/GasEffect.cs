using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEffect : MonoBehaviour
{
    Health playerHealth;
    public float inTimer = 0;
    public float damagePercent = .1f;
    // Start is called before the first frame update


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth = other.gameObject.GetComponent<Health>();
            inTimer += Time.deltaTime;
            if (inTimer >= 1f)
            {
                playerHealth.Damage(playerHealth.m_MaxHealth * damagePercent);
                inTimer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTimer = 0;

        }
    }
}
