using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // The enemy prefab to spawn
    public Vector3Object spawnPosition; // ScriptableObject holding the spawn position
    public float spawnInterval = 2f;    // Time between spawns

    private void Start()
    {
        // Check if the spawnPosition ScriptableObject is assigned
        if (spawnPosition == null)
        {
            Debug.LogError("Spawn position ScriptableObject is not assigned!");
            return;
        }

        // Start spawning enemies repeatedly
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnPosition == null) return;

        // Spawn the enemy at the position defined by the Vector3Object ScriptableObject
        Instantiate(enemyPrefab, spawnPosition.value, Quaternion.identity);
    }
}