using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform m_PlayerPos;

    [System.Serializable]
    public class Wave
    {
        public Subwave[] subWaves;
    }
    
   /* [Space(15)]//this is just spacing out the inspector to give better clarity*/

    [System.Serializable]
    public class Subwave
    {
        public string waveName;
        //this is as a transform, could also have it as a gameobject refrence. just drag the prefab you want to spawn onto this variable in the array.
        public Transform enemy;
        public Transform spawnPoints;
        public int count;
        public float rate;
    }

    //created this simple class to be able to name the spawn points so you dont have to remember which location is which and you can refrence the name when calling it.
    //could also just have it as an array of transforms, but have to make sure that you remember the correct index for each transform.
    public Wave[] newWaves;
    
   


    [Space(15)]
    public float m_timeBetweenWaves = 5.0f;
    private float m_timeSinceSpawn;
    private int waveNumber = 0;
    public float searchCountdown = 1f;
    private SpawnerState state = SpawnerState.counting;

    private void Start()
    {
        m_timeSinceSpawn = m_timeBetweenWaves;
    }

    private void Update()
    {
       
        if (state == SpawnerState.waiting)
        {
            //check if enemy is dead
            if(!EnemyIsAlive())
            {
                //if enemies are dead, start next wave
                WaveComplete();
            }
            else
            {
                //if there are some alive, return to skip spawning new ones and keep waiting for dead enemies.
                return;
            }
        }

        if(m_timeSinceSpawn <= 0)
        {
            //if its not currently spawning, spawn the next wave.
            if(state != SpawnerState.spawning)
            {
                if (newWaves.Length > waveNumber)
                     SpawnWave(newWaves[waveNumber]);
            }
        }
        else
        {
            //
            m_timeSinceSpawn -= Time.deltaTime;
        }
    }

    void WaveComplete()
    {
        state = SpawnerState.counting;
        m_timeSinceSpawn = m_timeBetweenWaves;


        if(waveNumber + 1 > newWaves.Length - 1)
        {
            //completed all waves
            Debug.Log("all waves completed");
            gameObject.SendMessage("EndOfWave");
        }

        waveNumber++; 
    }


   

    //ienum to allow the calling for the coroutine to have the delay
   public void SpawnWave(Wave _enemyWave)
   {
       state = SpawnerState.spawning;
       for(int i = 0; i < _enemyWave.subWaves.Length; i++)
       {

            StartCoroutine(SpawnEnemy(_enemyWave.subWaves[i]));
       }
        state = SpawnerState.waiting; 
       
   }
    IEnumerator SpawnEnemy(Subwave enemyToSpawn)
    {
        Debug.Log("Spawning Enemy: ");

            for (int i = 0; i < enemyToSpawn.count; i++)
        {
            Transform spawnedObject = Instantiate(enemyToSpawn.enemy, enemyToSpawn.spawnPoints.position, transform.rotation);
            if (spawnedObject.GetComponent<EnemyAI>())
                spawnedObject.GetComponent<EnemyAI>().m_PlayerPos = m_PlayerPos;

            yield return new WaitForSeconds(enemyToSpawn.rate);
        }
    }


    public bool EnemyIsAlive()
    {
        //countdown for the check each second.
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            //reset the countdown
            searchCountdown = 1f;

            //search through all game objects, looking for the enemy tag
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                
                //if nothing was found return false.
                return false;

            }
        }
       //if enemies are alive returning true
       return true;
       
    }
}

public enum SpawnerState
{
    //states for when enemies are spawning, counting down for next wave and waiting for the enemeies to die.
    spawning, counting, waiting
} 