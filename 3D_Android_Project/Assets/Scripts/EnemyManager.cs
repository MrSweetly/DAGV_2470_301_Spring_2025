using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // Singleton instance
    public Image enemyFillBar; // Assign in Inspector
    private int totalEnemies;
    private int remainingEnemies;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // End of if else
    }

    public void SetTotalEnemies(int total)
    {
        totalEnemies = total;
        remainingEnemies = total;
        UpdateFillBar();
    }

    public void EnemyDied()
    {
        remainingEnemies--;
        UpdateFillBar();
    }

    private void UpdateFillBar()
    {
        if (enemyFillBar)
        {
            enemyFillBar.fillAmount = (float)remainingEnemies / totalEnemies;
        }
        // End of if
    }
}