using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 5f;
    public int laneID;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float nextFireTime;

    private void OnTriggerEnter(Collider other)
    {
        TowerEnemy enemy = other.GetComponent<TowerEnemy>();
        if (enemy != null && enemy.laneID == laneID) // Only add enemies in the same lane
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
        // End of if
    }

    void Update()
    {
        enemiesInRange.RemoveAll(enemy => !enemy);

        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            FireAtEnemy();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void FireAtEnemy()
    {
        if (enemiesInRange.Count == 0) return;
        // End of if

        // Remove any null or destroyed enemies before targeting
        enemiesInRange.RemoveAll(enemy => !enemy || enemy.GetComponent<TowerEnemy>().health <= 0);

        if (enemiesInRange.Count == 0) return;
        // End of if

        GameObject target = enemiesInRange[0];

        if (target == null) return;
        // End of if

        // Ensure tower doesn't fire at enemies behind it
        if (target.transform.position.x < transform.position.x) return;
        // End of if

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
        // End of if else
    }
}