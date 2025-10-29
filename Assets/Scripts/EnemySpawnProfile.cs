using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class Wave {
    public List<GameObject> EnemiesForWave;
}

[CreateAssetMenu(fileName = "EnemySpawnProfile", menuName = "ScriptableObjects/EnemySpawnProfile", order = 1)]
public class EnemySpawnProfile : ScriptableObject
{
    public List<Wave> waves = new List<Wave>();
}
