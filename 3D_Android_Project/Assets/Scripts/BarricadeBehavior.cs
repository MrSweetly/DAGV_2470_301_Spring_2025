using UnityEngine;

public class BarricadeBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if (enemy != null)
            {
                enemy.SlowDown(0.5f); // Slow down enemy to 50% speed
                Debug.Log($"Enemy slowed down: {enemy.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if (enemy != null)
            {
                enemy.ResetSpeed(); // Restore original speed
                Debug.Log($"Enemy speed reset: {enemy.name}");
            }
        }
    }
}