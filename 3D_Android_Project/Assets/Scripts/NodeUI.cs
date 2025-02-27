using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private NodeControl target;
    public GameObject ui;

    // Reference to the PauseManager
    public PauseManager pauseManager;

    public void SetTarget(NodeControl target)
    {
        if (EnemyManager.Instance != null && EnemyManager.Instance.IsGameOver)
        {
            return; // Prevent selection if game is over
        }

        // Only activate SetTarget if the game is not paused
        if (pauseManager != null && pauseManager.IsPaused)
        {
            return; // Do nothing if the game is paused
        }

        if (target.HasTurret())  // Check if there's already a turret on this node
        {
            this.target = target;
            transform.position = target.GetBuildPosition();
            ui.SetActive(true); // Show the UI panel
        }
    }

    public void Hide()
    {
        ui.SetActive(false); // Hide the UI panel
    }
}

