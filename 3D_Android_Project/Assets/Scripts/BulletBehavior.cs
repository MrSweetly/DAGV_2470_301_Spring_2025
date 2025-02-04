using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private GameObject target;
    private Vector3 lastDirection;

    public void SetTarget(GameObject enemy)
    {
        target = enemy;
        if (target != null)
        {
            lastDirection = (target.transform.position - transform.position).normalized;
        }
        // End of if
    }
    // End of SetTarget

    void Update()
    {
        if (target == null)
        {
            // Continue moving in the last known direction
            transform.position += lastDirection * speed * Time.deltaTime;
            Destroy(gameObject, 2f);
            return;
        }
        // End of if

        // Regular movement towards the target
        lastDirection = (target.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        // If close enough, apply damage and destroy the bullet
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            if (target.GetComponent<TowerEnemy>() != null)
            {
                target.GetComponent<TowerEnemy>().TakeDamage(damage);
            }
            // End of if
            Destroy(gameObject);
        }
        // End of if
    }
    // End of Update
}