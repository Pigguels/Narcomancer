using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public Transform m_PlayerPos;

    private int m_CurrentEnemyCount = 0;

    void Awake()
    {
        if (!m_PlayerPos)
            m_PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;

        EnemyAI[] childEnemyAIs = GetComponentsInChildren<EnemyAI>();

        m_CurrentEnemyCount = childEnemyAIs.Length;

        foreach (EnemyAI enemyAI in childEnemyAIs)
        {
            enemyAI.m_PlayerPos = m_PlayerPos;
            enemyAI.m_WaveManager = this;
        }
    }

    public int GetCurrentEnemyCount() { return m_CurrentEnemyCount; }

    public void DecreaseEnemyCount()
    {
        Debug.Log(name + " decreased current enemy count to: " + m_CurrentEnemyCount.ToString());
        m_CurrentEnemyCount--;

        if (m_CurrentEnemyCount <= 0)
        {
            Debug.Log(name + " wave has ended");
            SendMessage("EndOfWave");
        }
    }
}
