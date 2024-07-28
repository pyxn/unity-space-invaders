using UnityEngine;
using System.Collections;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float minMoveSpeed = 1f;
    [SerializeField] private float maxMoveSpeed = 3f;
    [SerializeField] private float minRestTime = 1f;
    [SerializeField] private float maxRestTime = 5f;
    [SerializeField] private float minWalkTime = 2f;
    [SerializeField] private float maxWalkTime = 8f;

    private float currentSpeed;
    private float currentDirection;
    private bool isResting = false;

    private void Start()
    {
        StartCoroutine(MovementRoutine());
    }

    private void Update()
    {
        if (!isResting)
        {
            // Calculate movement for this frame
            float movement = currentSpeed * currentDirection * Time.deltaTime;

            // Move the object
            transform.Translate(new Vector2(movement, 0));
        }
    }

    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            // Walking phase
            StartNewMovement();
            isResting = false;
            yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));

            // Resting phase
            isResting = true;
            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        }
    }

    private void StartNewMovement()
    {
        // Set random speed within the defined range
        currentSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        // Set random direction (left or right)
        currentDirection = Random.value > 0.5f ? 1f : -1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the "Boundary" tag
        if (other.CompareTag("Boundary"))
        {
            Debug.Log("2D Trigger Enter: Hit Boundary " + other.gameObject.name);
            
            // Reverse the direction
            ReverseDirection();
        }
        // Ignore collisions with objects of the same type
        else if (other.GetComponent<TargetMovement>() != null)
        {
            // Do nothing, ignore the collision
            return;
        }
        // Optional: Log collisions with other objects
        else
        {
            Debug.Log("2D Trigger Enter: Hit " + other.gameObject.name + " (ignored)");
        }
    }

    private void ReverseDirection()
    {
        // Multiply the current direction by -1 to reverse it
        currentDirection *= -1f;
    }
}