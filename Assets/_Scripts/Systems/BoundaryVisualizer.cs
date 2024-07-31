using UnityEngine;

[RequireComponent(typeof(BoidManager))]
public class BoundaryVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BoidManager boidManager;

    void Start()
    {
        boidManager = GetComponent<BoidManager>();
        
        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5; // 5 points to close the rectangle
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        UpdateBoundaryVisualization();
    }

    void UpdateBoundaryVisualization()
    {
        Rect bounds = boidManager.spawnBounds;

        Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(bounds.xMin, bounds.yMin, 0);
        positions[1] = new Vector3(bounds.xMax, bounds.yMin, 0);
        positions[2] = new Vector3(bounds.xMax, bounds.yMax, 0);
        positions[3] = new Vector3(bounds.xMin, bounds.yMax, 0);
        positions[4] = positions[0]; // Close the loop

        lineRenderer.SetPositions(positions);
    }

    void OnValidate()
    {
        if (lineRenderer != null && boidManager != null)
        {
            UpdateBoundaryVisualization();
        }
    }
}