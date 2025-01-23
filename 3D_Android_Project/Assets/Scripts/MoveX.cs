using UnityEngine;

public class MoveX : MonoBehaviour
{
    public float speed = 5f;
    public int direction = 1;
    private Rigidbody rb;

    private bool isMoving = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing. Please add a Rigidbody to the object.");
        }
        // End of if
    }

    private void FixedUpdate()
    {
        if (rb && isMoving)
        {
            Vector3 moveAmount = new Vector3(direction * speed, 0, 0);
            rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
        }
        // End of if
    }
    // End of FixedUpdate

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
    }
    // End of OnCollisionEnter

    private void OnCollisionExit(Collision collision)
    {
        isMoving = true;
    }
    // End of OnCollisionExit
}