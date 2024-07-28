using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float velocity = 13f;

    [SerializeField] private float lifetime = 5f;

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
        gameObject.transform.Translate(Vector2.up * velocity * Time.deltaTime);
    }
}
