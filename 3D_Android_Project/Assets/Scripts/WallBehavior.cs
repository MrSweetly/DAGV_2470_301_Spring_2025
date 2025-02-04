using UnityEngine;

public class Wall : MonoBehaviour
{
    public int health = 100;
    public GameAction wallCollapseAction;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            wallCollapseAction?.RaiseAction();
        }
        // End of if
    }
}