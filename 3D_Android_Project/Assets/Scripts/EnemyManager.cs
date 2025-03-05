using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene reloading

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    public Image enemyFillBar;
    public GameObject levelFinishedPanel;
    public Button restartButton;

    private int totalEnemies, remainingEnemies;
    private bool isGameOver = false;
    
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        levelFinishedPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
    }

    public void SetTotalEnemies(int total)
    {
        totalEnemies = remainingEnemies = total;
        UpdateFillBar();
    }

    public void EnemyDied()
    {
        if (--remainingEnemies <= 0) ShowLevelCompleteScreen();
        UpdateFillBar();
    }

    private void UpdateFillBar()
    {
        enemyFillBar.fillAmount = totalEnemies > 0 ? Mathf.Clamp01((float)remainingEnemies / totalEnemies) : 0f;
    }

    private void ShowLevelCompleteScreen()
    {
        isGameOver = true;
        levelFinishedPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}