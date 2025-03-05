using UnityEngine;
using System.Collections.Generic;

public class TowerBehavior : MonoBehaviour
{
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 5f;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float nextFireTime;

    // Removed laneID dependency

    private void OnTriggerEnter(Collider other)
    {
        TowerEnemy enemy = other.GetComponent<TowerEnemy>();
        if (enemy != null) // Add any enemy to the range regardless of lane
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void Update()
    {
        // Clean up list, removing destroyed enemies
        enemiesInRange.RemoveAll(enemy => !enemy);

        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            FireAtEnemy();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireAtEnemy()
    {
        if (enemiesInRange.Count == 0) return;

        // Remove null or destroyed enemies before targeting
        enemiesInRange.RemoveAll(enemy => !enemy || enemy.GetComponent<TowerEnemy>().health <= 0);

        if (enemiesInRange.Count == 0) return;

        GameObject target = enemiesInRange[0]; // Choose the first valid enemy in range

        if (target == null) return;

        // Instantiate the bullet at the firePoint and target the chosen enemy
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BulletBehavior bulletScript = bullet.GetComponent<BulletBehavior>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
        else
        {
            Debug.LogError("BulletBehavior is missing on the Bullet prefab!");
        }
    }
}