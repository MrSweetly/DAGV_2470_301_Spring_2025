using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // For UI text

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public List<WavePattern> wavePatterns;
    public List<Vector3Object> spawnPositions;
    public TextMeshProUGUI countdownText; // Reference to UI countdown

    private int totalEnemies;
    private int spawnedEnemies;
    private List<WavePattern> runtimeWavePatterns;
    private float countdown = 5f; // 5-second timer before starting the round

    private void Start()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions assigned!");
            return;
        }
        if (wavePatterns == null || wavePatterns.Count == 0)
        {
            Debug.LogError("WavePatterns are not assigned!");
            return;
        }

        // Create runtime copies to preserve original data
        runtimeWavePatterns = new List<WavePattern>();
        foreach (var wavePattern in wavePatterns)
        {
            runtimeWavePatterns.Add(Instantiate(wavePattern)); // Copy each wave
        }

        // Sum all enemies from copied WavePatterns
        totalEnemies = 0;
        foreach (var wavePattern in runtimeWavePatterns)
        {
            foreach (var enemy in wavePattern.enemiesToSpawn)
            {
                totalEnemies += enemy.count;
            }
        }

        // Notify EnemyManager about total enemy count
        EnemyManager.Instance.SetTotalEnemies(totalEnemies);

        // Start countdown before spawning waves
        StartCoroutine(StartCountdown());
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // **New Coroutine for Countdown Timer**
    private IEnumerator StartCountdown()
    {
        while (countdown > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = "Wave starts in: " + Mathf.Ceil(countdown).ToString();
            }
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        if (countdownText != null)
        {
            countdownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            countdownText.text = ""; // Clear text after countdown
        }

        // Start enemy spawning after countdown
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        foreach (var wavePattern in runtimeWavePatterns)
        {
            yield return StartCoroutine(SpawnEnemiesOverTime(wavePattern));
        }
    }

    private IEnumerator SpawnEnemiesOverTime(WavePattern wavePattern)
    {
        float spawnRate = wavePattern.spawnDuration / totalEnemies;
        int index = 0;

        while (index < wavePattern.enemiesToSpawn.Count)
        {
            var currentEnemy = wavePattern.enemiesToSpawn[index];

            if (currentEnemy.count > 0)
            {
                int laneIndex = Random.Range(0, spawnPositions.Count);
                Vector3 spawnPos = spawnPositions[laneIndex].value;

                GameObject enemyObj = Instantiate(currentEnemy.enemyPrefab, spawnPos, Quaternion.identity);
                TowerEnemy enemy = enemyObj.GetComponent<TowerEnemy>();
                if (enemy != null)
                {
                    enemy.SetLane(laneIndex);
                }

                spawnedEnemies++;
                currentEnemy.count--;

                yield return new WaitForSeconds(spawnRate);
            }

            if (currentEnemy.count == 0) index++;
        }
    }
}