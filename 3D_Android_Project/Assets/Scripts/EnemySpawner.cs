using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<WavePattern> wavePatterns; // Support multiple waves
    public List<Vector3Object> spawnPositions; // List of spawn locations as ScriptableObjects

    private int totalEnemies;
    private int spawnedEnemies;
    private List<WavePattern> runtimeWavePatterns;

    private void Start()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions assigned!");
            return;
        }
        // End of if
        if (wavePatterns == null || wavePatterns.Count == 0)
        {
            Debug.LogError("WavePatterns are not assigned!");
            return;
        }
        // End of if

        // Create runtime copies to preserve original data
        runtimeWavePatterns = new List<WavePattern>();
        foreach (var wavePattern in wavePatterns)
        {
            runtimeWavePatterns.Add(Instantiate(wavePattern)); // Copy each wave
        }
        // End of foreach

        // Sum all enemies from copied WavePatterns
        totalEnemies = 0;
        foreach (var wavePattern in runtimeWavePatterns)
        {
            foreach (var enemy in wavePattern.enemiesToSpawn)
            {
                totalEnemies += enemy.count;
            }
            // End of foreach
        }
        // End of foreach

        // Notify EnemyManager about the correct total enemy count
        EnemyManager.Instance.SetTotalEnemies(totalEnemies);

        StartCoroutine(SpawnAllWaves());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnAllWaves()
    {
        foreach (var wavePattern in runtimeWavePatterns) // Use copies, not originals
        {
            yield return StartCoroutine(SpawnEnemiesOverTime(wavePattern));
        }
        // End of foreach
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnEnemiesOverTime(WavePattern wavePattern)
    {
        float spawnRate = wavePattern.spawnDuration / totalEnemies;
        int index = 0;

        while (index < wavePattern.enemiesToSpawn.Count)
        {
            var currentEnemy = wavePattern.enemiesToSpawn[index];

            if (currentEnemy.count > 0)
            {
                // Choose a random lane
                int laneIndex = Random.Range(0, spawnPositions.Count);
                Vector3 spawnPos = spawnPositions[laneIndex].value;

                // Spawn enemy and assign lane
                GameObject enemyObj = Instantiate(currentEnemy.enemyPrefab, spawnPos, Quaternion.identity);
                TowerEnemy enemy = enemyObj.GetComponent<TowerEnemy>();
                if (enemy != null)
                {
                    enemy.SetLane(laneIndex); // Assign correct lane
                }
                // End of if

                spawnedEnemies++;
                currentEnemy.count--;

                yield return new WaitForSeconds(spawnRate);
            }
            // End of if

            if (currentEnemy.count == 0) index++;
            // End of if
        }
        // End of while
    }
}
