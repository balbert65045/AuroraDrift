using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject LevelupBlock;
    [SerializeField] GameObject BlackHole;
    [SerializeField] EnemySpawnProfile profile;


    [SerializeField] float Radius = 20f;
    [SerializeField] float SpawnRate = 2f;


    PlayerMovement pm;
    int currentWaveIndex = 0;

    public List<GameObject> enemiesOut = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        StartCoroutine("WaitThenSpawnInitalWave");
        //Spawn(profile.waves[currentWaveIndex]);
    }

    IEnumerator WaitThenSpawnInitalWave()
    {
        yield return new WaitForSeconds(1);
        Spawn(profile.waves[currentWaveIndex]);

    }

    public void SpawnNextWave()
    {
        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        float spawnRadius = Radius / 3;
        Instantiate(BlackHole, (Vector2)pm.transform.position + (randomDir * spawnRadius), Quaternion.identity);

        //Spawn(profile.waves[currentWaveIndex]);

/*
        if (enemiesOut.Count == 0)
        {
            Spawn(profile.waves[currentWaveIndex]);
        }
*/
    }

    void Spawn(Wave wave)
    {
        LastEnemyDestroyed = false;
        currentWaveIndex++;

        foreach (GameObject enemyToSpawn in wave.EnemiesForWave)
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

            float spawnRadius = Radius;
           
            GameObject spawn = Instantiate(enemyToSpawn, (Vector2)pm.transform.position + (randomDir * spawnRadius), Quaternion.identity);
            enemiesOut.Add(spawn);
            if (spawn.GetComponentInChildren<EnemyHealth>())
            {
                spawn.GetComponentInChildren<EnemyHealth>().OnDeath += OnEnemyDestroyed;
            }
        }
    }

    bool LastEnemyDestroyed = false;
    void OnEnemyDestroyed(object sender, GameObject enemy)
    {
        enemiesOut.Remove(enemy);
        if (enemiesOut.Count == 0 && !LastEnemyDestroyed)
        {
            LastEnemyDestroyed = true;
            if (currentWaveIndex >= profile.waves.Count)
            {
                //All waves done -> Level Up
                //FindObjectOfType<GameManager>().CompleteLevel();

                Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
                float spawnRadius = Radius / 3;
                Instantiate(LevelupBlock, (Vector2)pm.transform.position + (randomDir * spawnRadius), Quaternion.identity);

            }
            else
            {
                Spawn(profile.waves[currentWaveIndex]);
            }
        }
    }

    IEnumerator WaitThenLevelThenSpawn()
    {
        yield return new WaitForSeconds(.4f);
        if (FindObjectOfType<UpgradeSystem>())
        {
            FindObjectOfType<UpgradeSystem>().ShowPossibleUpgrades();
        }
        yield return new WaitForSeconds(.2f);
        Spawn(profile.waves[currentWaveIndex]);
    }
}
