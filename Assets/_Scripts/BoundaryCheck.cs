using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    public Rect bounds;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boid"))
        {
            Vector2 position = collision.transform.position;
            if (position.x < bounds.xMin) position.x = bounds.xMax;
            if (position.x > bounds.xMax) position.x = bounds.xMin;
            if (position.y < bounds.yMin) position.y = bounds.yMax;
            if (position.y > bounds.yMax) position.y = bounds.yMin;
            collision.transform.position = position;
        }
    }
}