using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float m_timeToSpawn = 5.0f;
    public float m_timeSinceSpawn;

    public GameObject enemyToSpawn;

    private void Update()
    {
        m_timeSinceSpawn += Time.deltaTime;
        if(m_timeSinceSpawn >= m_timeToSpawn)
        {
            GameObject newEnemy = Instantiate(enemyToSpawn);
            newEnemy.transform.position = this.transform.position;
            m_timeSinceSpawn = 0f;
        }
    }
}
