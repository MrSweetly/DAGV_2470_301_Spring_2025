using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public WavePattern wavePattern;
    public Vector3Object spawnPosition;

    private WavePattern runtimeWavePattern;
    private float totalEnemies;
    private float elapsedTime;

    private void Start()
    {
        if (spawnPosition == null)
        {
            Debug.LogError("Spawn position ScriptableObject is not assigned!");
            return;
        }
        if (wavePattern == null)
        {
            Debug.LogError("WavePattern ScriptableObject is not assigned!");
            return;
        }

        // Create a runtime copy of the WavePattern to preserve the original values
        runtimeWavePattern = Instantiate(wavePattern);

        totalEnemies = 0;
        foreach (var enemy in runtimeWavePattern.enemiesToSpawn)
        {
            totalEnemies += enemy.count;
        }

        StartCoroutine(SpawnEnemiesOverTime());
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        float spawnRate = runtimeWavePattern.spawnDuration / totalEnemies;
        int index = 0;

        while (elapsedTime < runtimeWavePattern.spawnDuration)
        {
            if (index >= runtimeWavePattern.enemiesToSpawn.Count) break;

            var currentEnemy = runtimeWavePattern.enemiesToSpawn[index];

            if (currentEnemy.count > 0)
            {
                Instantiate(currentEnemy.enemyPrefab, spawnPosition.value, Quaternion.identity);
                currentEnemy.count--;
                elapsedTime += spawnRate;
                yield return new WaitForSeconds(spawnRate);
            }

            if (currentEnemy.count == 0) index++;
        }
    }
}