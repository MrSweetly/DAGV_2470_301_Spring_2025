using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 50;
    public float speed = 5f;
    private float originalSpeed;
    public int direction = 1;
    public int damage = 10;
    public float attackRate = 1f;

    private Rigidbody rb;
    private bool isMoving = true;
    private Wall targetWall;
    private float nextAttackTime;

    private Vector3 movementDirection; // Cached movement direction for efficiency

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"[{gameObject.name}] Rigidbody component is missing! Add a Rigidbody to fix this.");
            return;
        }

        originalSpeed = speed;
        UpdateMovementDirection(); // Initialize the movement direction with the current speed
    }

    private void UpdateMovementDirection()
    {
        movementDirection = new Vector3(direction * speed, 0, 0); // Update movement direction based on current speed
    }

    private void FixedUpdate()
    {
        if (!isMoving || targetWall != null) return; // Skip movement if stopped or near a wall

        rb.MovePosition(rb.position + movementDirection * Time.fixedDeltaTime); // Apply movement
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Wall")) return;

        targetWall = other.GetComponent<Wall>();
        if (targetWall == null)
        {
            Debug.LogError($"[{gameObject.name}] Wall object is missing Wall script!");
            return;
        }

        isMoving = false; // Stop moving upon wall contact
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetWall == null || Time.time < nextAttackTime) return;

        targetWall.TakeDamage(damage);
        nextAttackTime = Time.time + attackRate;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Wall")) return;

        targetWall = null;
        isMoving = true;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health > 0) return;

        EnemyManager.Instance?.EnemyDied();
        Destroy(gameObject);
    }

    public void SlowDown(float factor)
    {
        speed = originalSpeed * factor; // Slow down the enemy by the given factor
        UpdateMovementDirection(); // Recalculate movement direction with new speed
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
        UpdateMovementDirection(); // Recalculate movement direction with original speed
    }
}
