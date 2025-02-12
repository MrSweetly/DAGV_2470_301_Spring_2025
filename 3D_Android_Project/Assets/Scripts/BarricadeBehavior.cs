using UnityEngine;

public class BarricadeBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TowerEnemy enemy = other.GetComponent<TowerEnemy>();
        if (enemy != null)
        {
            enemy.SlowDown(0.5f); // Slow enemy to 50% speed
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TowerEnemy enemy = other.GetComponent<TowerEnemy>();
        if (enemy != null)
        {
            enemy.ResetSpeed(); // Restore original speed
        }
    }
}
