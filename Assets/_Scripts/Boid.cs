using System;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public event EventHandler OnBoidDestruction;
    public float speed = 5f;
    public float neighborRadius = 2f;
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    private Vector2 velocity;
    private Rect bounds;
    private float lifetime = 0f;
    private bool hasCloned = false;
    public GameObject boidPrefab; // Assign this in the Unity Inspector

    private void Start()
    {
        velocity = UnityEngine.Random.insideUnitCircle.normalized * speed;
        bounds = FindAnyObjectByType<BoidManager>().spawnBounds;
    }


    private void Update()
    {

        // Increment the lifetime
        lifetime += Time.deltaTime;

        // Check if it's time to clone and hasn't cloned yet
        if (lifetime > 13f && !hasCloned)
        {
            CloneBoid();
            hasCloned = true;
        }

        Vector2 separation = Vector2.zero;
        Vector2 alignment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        int neighborCount = 0;

        foreach (Boid other in FindObjectsByType<Boid>(FindObjectsSortMode.None))
        {
            if (other != this)
            {
                float distance = Vector2.Distance(transform.position, other.transform.position);
                if (distance < neighborRadius)
                {
                    separation += (Vector2)(transform.position - other.transform.position).normalized / distance;
                    alignment += (Vector2)other.velocity;
                    cohesion += (Vector2)other.transform.position;
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            separation /= neighborCount;
            alignment /= neighborCount;
            cohesion /= neighborCount;

            cohesion = (cohesion - (Vector2)transform.position).normalized;

            Vector2 steering = separation * separationWeight +
                               alignment.normalized * alignmentWeight +
                               cohesion * cohesionWeight;

            velocity = (velocity + steering).normalized * speed;
        }

        // Move the boid
        Vector2 newPosition = (Vector2)transform.position + velocity * Time.deltaTime;

        // Check and resolve boundary collisions
        if (newPosition.x < bounds.xMin)
        {
            newPosition.x = bounds.xMin;
            velocity.x = -velocity.x;
        }
        else if (newPosition.x > bounds.xMax)
        {
            newPosition.x = bounds.xMax;
            velocity.x = -velocity.x;
        }

        if (newPosition.y < bounds.yMin)
        {
            newPosition.y = bounds.yMin;
            velocity.y = -velocity.y;
        }
        else if (newPosition.y > bounds.yMax)
        {
            newPosition.y = bounds.yMax;
            velocity.y = -velocity.y;
        }

        // Update position and rotation
        transform.position = newPosition;
        transform.up = velocity;
    }

    private void CloneBoid()
    {
        if (boidPrefab != null)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f; // Small random offset
            Vector3 spawnPosition = transform.position + (Vector3)offset;
            Instantiate(boidPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Boid prefab is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            ProjectileImpact(other.gameObject);
        }
    }


    // Exploding
    [SerializeField] private GameObject explosionPrefab;
    private int currentHits = 0;
    private event EventHandler OnProjectileHit;
    [SerializeField] private int maxHits = 1;
    private void ProjectileImpact(GameObject projectile)
    {
        currentHits++;
        OnProjectileHit?.Invoke(this, EventArgs.Empty);
        Destroy(projectile);

        if (currentHits >= maxHits)
        {
            OnBoidDestruction?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}