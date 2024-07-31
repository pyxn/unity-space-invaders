using UnityEngine;
using System;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int maxHits = 10;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float separationWeight = 1f;
    [SerializeField] private float neighborhoodRadius = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 2f;

    private int currentHits = 0;
    private Vector2 velocity;

    public event EventHandler OnProjectileHit;

    private void Start()
    {
        // Spawn above the camera view
        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        transform.position = spawnPosition;

        // Initialize with a random velocity
        velocity = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        Vector2 acceleration = CalculateBoidBehavior();
        velocity += acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // Ensure minimum speed
        if (velocity.magnitude < minSpeed)
        {
            velocity = velocity.normalized * minSpeed;
        }

        transform.position += (Vector3)velocity * Time.deltaTime;

        // Rotate the target to face its movement direction
        if (velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector2 CalculateBoidBehavior()
    {
        Vector2 cohesion = Vector2.zero;
        Vector2 alignment = Vector2.zero;
        Vector2 separation = Vector2.zero;
        int neighborCount = 0;

        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, neighborhoodRadius);

        foreach (Collider2D neighbor in neighbors)
        {
            TargetBehaviour neighborBoid = neighbor.GetComponent<TargetBehaviour>();
            if (neighborBoid != null && neighborBoid != this)
            {
                cohesion += (Vector2)neighbor.transform.position;
                alignment += neighborBoid.velocity;
                separation += (Vector2)(transform.position - neighbor.transform.position).normalized /
                    Vector2.Distance(transform.position, neighbor.transform.position);
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            cohesion = (cohesion / neighborCount - (Vector2)transform.position) * cohesionWeight;
            alignment = (alignment / neighborCount - velocity) * alignmentWeight;
            separation = separation * separationWeight;
        }

        Vector2 boundaryAvoidance = AvoidBoundary() * 2f; // Adjust the weight as needed

        return cohesion + alignment + separation + boundaryAvoidance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            // Calculate the normal of the boundary
            Vector2 closestPoint = other.ClosestPoint(transform.position);
            Vector2 normal = ((Vector2)transform.position - closestPoint).normalized;

            // Reflect the velocity off the boundary
            velocity = Vector2.Reflect(velocity, normal);

            // Move the boid slightly away from the boundary to prevent sticking
            transform.position = (Vector2)transform.position + normal * 0.1f;
        }
        else if (other.CompareTag("Projectile"))
        {
            ProjectileImpact(other.gameObject);
        }
    }

    private Vector2 AvoidBoundary()
    {
        Vector2 avoidance = Vector2.zero;
        float avoidanceDistance = 2f; // Adjust this value as needed

        // Check distance to screen edges
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint((Vector2)transform.position);

        if (viewportPosition.x < avoidanceDistance)
            avoidance.x = 1;
        else if (viewportPosition.x > 1 - avoidanceDistance)
            avoidance.x = -1;

        if (viewportPosition.y < avoidanceDistance)
            avoidance.y = 1;
        else if (viewportPosition.y > 1 - avoidanceDistance)
            avoidance.y = -1;

        return avoidance.normalized;
    }

    private void ProjectileImpact(GameObject projectile)
    {
        currentHits++;
        OnProjectileHit?.Invoke(this, EventArgs.Empty);
        Destroy(projectile);

        if (currentHits >= maxHits)
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}