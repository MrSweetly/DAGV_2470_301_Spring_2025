using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public List<WavePattern> wavePatterns;
    public List<GameObject> spawnPositions;  // Changed to a list of GameObjects
    public Text countdownText;

    private int totalEnemies;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (wavePatterns.Count == 0 || spawnPositions.Count == 0)
        {
            Debug.LogError("Missing wave patterns or spawn positions!");
            return;
        }

        // Calculate total enemies in one go
        totalEnemies = 0;
        wavePatterns.ForEach(wave => wave.enemiesToSpawn.ForEach(enemy => totalEnemies += enemy.count));

        EnemyManager.Instance.SetTotalEnemies(totalEnemies);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        for (int countdown = 5; countdown > 0; countdown--)
        {
            if (countdownText != null)
                countdownText.text = $"WAVE STARTS IN: {countdown}";
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
        {
            countdownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            countdownText.text = "";
        }

        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        foreach (var wave in wavePatterns)
            yield return StartCoroutine(SpawnEnemiesOverTime(wave));
    }

    private IEnumerator SpawnEnemiesOverTime(WavePattern wave)
    {
        float spawnRate = wave.spawnDuration / totalEnemies;

        foreach (var enemyType in wave.enemiesToSpawn)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                // Use the position of the random spawn point from GameObjects
                Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)].transform.position;
                var enemyObj = Instantiate(enemyType.enemyPrefab, spawnPos, Quaternion.identity);

                // No need to set lane, just spawn enemies
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}

