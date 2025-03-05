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
        [HideInInspector] public int initialCount; // Store the original count
    }

    public List<EnemyType> enemiesToSpawn;
    public float spawnDuration = 60f;

    // Function to reset enemy counts when restarting the level
    public void ResetEnemies()
    {
        foreach (var enemy in enemiesToSpawn)
        {
            enemy.count = enemy.initialCount;
        }
    }

    // Function to store initial enemy count at the start of the game
    public void StoreInitialCounts()
    {
        foreach (var enemy in enemiesToSpawn)
        {
            enemy.initialCount = enemy.count;
        }
    }
}