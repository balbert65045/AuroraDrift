using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> SpawnObjects;
    [SerializeField] List<float> Rarities;


    [SerializeField] float Radius = 20f;
    [SerializeField] float SpawnRate = 2f;

    float timeSinceLastSpawn;

    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        //for(int i = 0; i < 4; i++)
        //{
        //    float x = 0;
        //    float y = 0;
        //    while (x == 0 && y == 0)
        //    {
        //        x = Random.Range(-1f, 1f);
        //        y = Random.Range(-1f, 1f);
        //    }
        //    Vector3 dir = new Vector3(x, y, 0);

        //    float ranRadius = Random.Range(50f, Radius);

        //    Instantiate(SpawnObjects[0], pm.transform.position + (dir.normalized * ranRadius), Quaternion.identity);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > timeSinceLastSpawn + SpawnRate)
        {
            SpawnRate -= -.1f;
            Mathf.Clamp(SpawnRate, .5f, 1000);
            Spawn();
            Spawn();
        }
    }

    void Spawn()
    {
        timeSinceLastSpawn = Time.timeSinceLevelLoad;
        float x = 0;
        float y = 0;
        while(x == 0 && y == 0)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);
        }
        Vector3 dir = new Vector3(x, y , 0);

        float RarityRoll = Random.Range(0f, 1f);
        int index = 0;
        for(int i = Rarities.Count - 1; i >= 0; i--)
        {
            if (RarityRoll <= Rarities[i])
            {
                index = i;
                break;
            }
        }

        //int index = Random.Range(0, SpawnObjects.Count);
        Instantiate(SpawnObjects[index], pm.transform.position + (dir.normalized * Radius), Quaternion.identity);
    }
}
