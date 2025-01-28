using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Wave Pattern")]

public class WavePattern : ScriptableObject
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public int count;
    }

    public List<EnemyType> enemiesToSpawn;
    public float spawnDuration = 60f;
}