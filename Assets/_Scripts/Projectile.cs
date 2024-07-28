using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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

    private float velocity = 13f;
    
    private void Update()
    {
        gameObject.transform.Translate(Vector2.up * velocity * Time.deltaTime);
    }

}
