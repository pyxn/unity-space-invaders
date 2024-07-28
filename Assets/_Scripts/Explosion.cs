using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
     [SerializeField] private float lifetime = 0.18f;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
