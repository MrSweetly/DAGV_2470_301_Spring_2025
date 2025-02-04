using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 5f;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float nextFireTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesInRange.Add(other.gameObject);
    }
    // End of if

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }
    // End of if

    void Update()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            FireAtEnemy();
            nextFireTime = Time.time + 1f / fireRate;
        }
        // End of if
    }
    // End of Update

    void FireAtEnemy()
    {
        if (enemiesInRange.Count == 0) return;
        // End of if

        // Remove any null or destroyed enemies before targeting
        enemiesInRange.RemoveAll(enemy => enemy == null || enemy.GetComponent<TowerEnemy>().health <= 0);
    
        if (enemiesInRange.Count == 0) return; // If no valid enemies, stop shooting
        // End of if

        GameObject target = enemiesInRange[0]; // Get the first valid enemy

        if (target == null) return; // Extra safety check
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
    // End of FireAtEnemy
}