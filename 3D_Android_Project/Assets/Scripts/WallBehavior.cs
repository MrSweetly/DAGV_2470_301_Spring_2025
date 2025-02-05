using UnityEngine;

public class Wall : MonoBehaviour
{
    public int health = 100;
    public GameAction wallCollapseAction;
    private bool hasCollapsed = false; // Prevents multiple triggers

    public void TakeDamage(int damage)
    {
        if (hasCollapsed) return; // Stop if already triggered
        // End of if

        health -= damage;

        if (health <= 0 && !hasCollapsed)
        {
            hasCollapsed = true; // Set flag to prevent duplicate activation
            wallCollapseAction?.RaiseAction();
        }
        // End of if
    }
}