﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GasEffect : MonoBehaviour
{
    Health playerHealth;
    public float inTimer = 0;
    public float damagePercent = .1f;
    public GameObject camera;
    Volume volume;
    public float maxWeight = 1f;
    public float lowWeight =.1f;
    public GameObject player;

    // Start is called before the first frame update

    private void Awake()
    {
        volume = camera.GetComponent<Volume>();
    }

    private void Update()
    {
        if (player.transform.position.y >= 4.5f)
        {
            volume.weight = Mathf.Lerp(volume.weight, 0, 1);
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            volume.weight = Mathf.Lerp(volume.weight, 1f, 2f);
            playerHealth = other.gameObject.GetComponent<Health>();
            inTimer += Time.deltaTime;
            if (inTimer >= 1f)
            {
                playerHealth.Damage(playerHealth.m_MaxHealth * damagePercent);
                inTimer = 0;
            }
        }
    }

    
}
