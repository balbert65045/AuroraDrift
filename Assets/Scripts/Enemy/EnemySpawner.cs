using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemySpawnProfile profile;
    [SerializeField] List<float> Rarities;


    [SerializeField] float Radius = 20f;
    [SerializeField] float SpawnRate = 2f;

    float timeSinceLastSpawn;

    PlayerMovement pm;
    int currentWaveIndex = 0;
    int enemiesAliveInWave;

    public List<GameObject> enemiesOut = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        Spawn(profile.waves[currentWaveIndex]);
    }


    void Spawn(Wave wave)
    {

        currentWaveIndex++;

        foreach (GameObject enemyToSpawn in wave.EnemiesForWave)
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            GameObject spawn = Instantiate(enemyToSpawn, (Vector2)pm.transform.position + (randomDir * Radius), Quaternion.identity);
            enemiesOut.Add(spawn);
            spawn.GetComponentInChildren<EnemyHealth>().OnDeath += OnEnemyDestroyed;
        }
    }

    void OnEnemyDestroyed(object sender, GameObject enemy)
    {
        enemiesOut.Remove(enemy);
        if (enemiesOut.Count == 0)
        {
            if(currentWaveIndex >= profile.waves.Count)
            {
                //All waves done -> Complete Level
                FindObjectOfType<GameManager>().CompleteLevel();
            }
            else
            {
                Spawn(profile.waves[currentWaveIndex]);
            }
        }
    }
}
