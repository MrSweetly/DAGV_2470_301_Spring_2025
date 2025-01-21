using UnityEngine;
using System.Collections;

public class MoveX : MonoBehaviour
{
    public float speed = 5f;
    public int direction = 1;
    private bool isMoving = true;
    private bool collisionResolved = false;

    private void Start()
    {
        StartCoroutine(MoveContinuously());
    }

    private IEnumerator MoveContinuously()
    {
        while (true)
        {
            switch (isMoving)
            {
                case true:
                    var moveAmount = direction * speed * Time.deltaTime;
                    transform.Translate(moveAmount, 0, 0);
                    break;
                
                case false:
                    break;
            }
            // End of switch
            yield return null;
        }
        // End of while
    }

    private void OnCollisionEnter(Collision collision) {
        isMoving = false; }

    private void OnCollisionExit(Collision collision) {
        isMoving = true; }
}