using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;
    public GameObject boidPrefab;
    public Rect spawnBounds;
    private float elapsedTime = 0f;
    private const float totalTime = 120f;
    private const float spawnInterval = 1.618f;
    private int boidCount = 0;
    private int initialBoidCount = 8;

    private void Start()
    {
        Instance = this;
        // Spawn initial 8 boids
        SpawnBoids(initialBoidCount);
        // Start the coroutine to spawn additional boids
        // StartCoroutine(SpawnBoidsOverTime());
    }

    private IEnumerator SpawnBoidsOverTime()
    {
        while (elapsedTime < totalTime)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBoids(1);
            elapsedTime += spawnInterval;
        }
    }

    private void SpawnBoids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnBounds.xMin, spawnBounds.xMax),
                Random.Range(spawnBounds.yMin, spawnBounds.yMax)
            );
            Instantiate(boidPrefab, randomPosition, Quaternion.identity);
            IncrementBoids();
        }
        // Debug.Log($"Current boid count: {currentBoidCount}");
    }

    public void IncrementBoids()
    {
        boidCount++;
    }

    public void DecrementBoids()
    {
        boidCount--;
    }

    public bool NoBoidsRemaining()
    {
        return boidCount <= 0;
    }

}