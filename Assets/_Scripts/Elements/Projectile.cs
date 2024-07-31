using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;
    private float speed;

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        transform.up = direction; // Rotate the projectile to face the direction
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    
    private void Update()
    {
        // Move the projectile in the initialized direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}