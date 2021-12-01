using System.Collections;
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
    public float volumeWeight;
    
    public GameObject player;

    // Start is called before the first frame update
    private void OnEnable()
    {
        Debug.LogError("GAS ON");
    }
    private void Awake()
    {
        volume = camera.GetComponent<Volume>();
        volumeWeight = volume.weight;
        playerHealth = player.gameObject.GetComponent<Health>();
    }

    private void Update()
    {
        if (player.transform.position.y >= 4.5f)
        {
            volume.weight = Mathf.Lerp(volume.weight, 0, .05f);
        }
       
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            volume.weight = Mathf.Lerp(volume.weight, 1f, 0.1f);
            if(volume.weight >=.9f)
            {
                volume.weight = 1f;

            }
            
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
            volume.weight = Mathf.Lerp(volume.weight, 0, .05f);
        }
        
    }


    private void OnDisable()
    {
        Debug.LogError("GAS DISABLED");
        //StartCoroutine(FadeVolume());
    }

    public IEnumerator FadeoutVol()
    {
        while (volume.weight > 0.0f)
        {
            volume.weight -= 3.0f * Time.deltaTime;
            volume.weight = Mathf.Clamp01(volume.weight);

            yield return null;
        }
    }
}
