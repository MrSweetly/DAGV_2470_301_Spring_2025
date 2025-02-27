using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene reloading

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public Image enemyFillBar;
    private int totalEnemies;
    private int remainingEnemies;
    public GameObject levelFinishedPanel;
    public Button restartButton;
    
    private bool isGameOver = false;

    public bool IsGameOver => isGameOver; // Public getter

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        levelFinishedPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
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
        isGameOver = true; // Set game over state
        levelFinishedPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
