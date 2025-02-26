using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene reloading

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // Singleton instance
    public Image enemyFillBar; // Assign in Inspector
    private int totalEnemies;
    private int remainingEnemies;

    public GameObject levelFinishedPanel; // Reference to Level Finished UI
    public Button restartButton; // Restart button reference

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        levelFinishedPanel.SetActive(false); // Ensure it's hidden at start
        restartButton.onClick.AddListener(RestartLevel); // Assign button action
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

        // Check if all enemies are defeated
        if (remainingEnemies <= 0)
        {
            ShowLevelCompleteScreen();
        }
    }

    private void UpdateFillBar()
    {
        if (enemyFillBar)
        {
            enemyFillBar.fillAmount = (float)remainingEnemies / totalEnemies;
        }
    }

    private void ShowLevelCompleteScreen()
    {
        levelFinishedPanel.SetActive(true); // Show UI
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    
    
}