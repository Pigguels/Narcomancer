using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    public Transform m_PlayerPos;

    public bool m_ShouldCallEndOfWave = true;

    public List<UnityEvent> m_Events;

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
            
            foreach (UnityEvent uEvent in m_Events)
            {
                uEvent.Invoke();
            }

            if (m_ShouldCallEndOfWave)
                gameObject.SendMessage("EndOfWave");
        }
    }

    public void EnableGameObject(GameObject theGameObject)
    {
        theGameObject.SetActive(true);
    }
}
