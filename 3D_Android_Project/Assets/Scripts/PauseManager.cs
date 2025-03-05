using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Button pauseButton; // Assign in Inspector
    public Text pauseText; // Assign in Inspector (Legacy Text)
    private Image buttonImage; // Reference to button's background image

    private bool isPaused = false;

    private Color normalButtonColor = Color.white; // Default: White background
    private Color normalTextColor = Color.black;   // Default: Black text
    private Color pausedButtonColor = Color.red;   // When paused: Red background
    private Color pausedTextColor = Color.white;   // When paused: White text

    public bool IsPaused => isPaused; // Public getter to access paused state

    void Start()
    {
        buttonImage = pauseButton.GetComponent<Image>(); // Get the button's Image component

        // Assign button listener
        pauseButton.onClick.AddListener(TogglePause);

        // Set initial colors
        UpdateButtonVisuals();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // Toggle pause

        UpdateButtonVisuals(); // Change button appearance
        SetUIInteractable(!isPaused); // Disable UI when paused
    }

    private void UpdateButtonVisuals()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isPaused ? pausedButtonColor : normalButtonColor; // Change button background
        }

        if (pauseText != null)
        {
            pauseText.color = isPaused ? pausedTextColor : normalTextColor; // Change text color
        }
    }

    private void SetUIInteractable(bool isInteractable)
    {
        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button btn in allButtons)
        {
            if (btn != pauseButton) // Keep the pause button active
            {
                btn.interactable = isInteractable;
            }
        }
    }
}