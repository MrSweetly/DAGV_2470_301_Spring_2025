using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    public int health = 50;
    public float speed = 5f;
    private float originalSpeed;
    public int direction = 1;
    public int damage = 10;
    public float attackRate = 1f;
    public int laneID; // Assign a lane to the enemy

    private Rigidbody rb;
    private bool isMoving = true;
    private Wall targetWall;
    private float nextAttackTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing. Please add a Rigidbody to the object.");
        }
        // End of if
        originalSpeed = speed;
    }

    public void SetLane(int lane) 
    {
        laneID = lane; // Set lane when spawned
    }


    private void FixedUpdate()
    {
        if (rb && isMoving && !targetWall)
        {
            Vector3 moveAmount = new Vector3(direction * speed, 0, 0);
            rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
        }
        // End of if
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall")) // Ensure walls have "Wall" tag
        {
            targetWall = other.GetComponent<Wall>();
            isMoving = false;
        }
        // End of if
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetWall != null && Time.time >= nextAttackTime)
        {
            targetWall.TakeDamage(damage);
            nextAttackTime = Time.time + attackRate;
        }
        // End of if
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            targetWall = null;
            isMoving = true;
        }
        // End of if
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            if (EnemyManager.Instance) 
            {
                EnemyManager.Instance.EnemyDied();
            }

            Destroy(gameObject);
        }
    }
    
    public void SlowDown(float factor)
    {
        speed = originalSpeed * factor; // Reduce speed
    }

    public void ResetSpeed()
    {
        speed = originalSpeed; // Restore original speed
    }
}