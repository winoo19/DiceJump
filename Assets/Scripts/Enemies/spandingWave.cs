using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class SpandingWave : MonoBehaviour
{
    public int points = 50; // Number of points in the circle
    private float lineWidth = 0.02f;
    private LineRenderer lineRenderer;
    private float actualRadius = 0f; // Actual radius of the circle
    private float objectiveRadius = 1f; // Radius of the circle when it will be destroyed
    private float speed = 5f; // Speed of the radius increment

    void Awake()
    {
        // Get the LineRenderer component and set its properties
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    void Update()
    {   
        if (actualRadius < objectiveRadius)
        {
            // diminish de radius
            UpdateRadius(actualRadius);
            actualRadius += speed*Time.deltaTime;
        }
        else
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }

    // Update the radius of the circle
    public void UpdateRadius(float radius)
    {
        for (int i = 0; i < points; i++)
        {
            float angle = i * (2 * Mathf.PI) / points;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        // Set the last point of the circle to the first point (to close the circle)
        lineRenderer.SetPosition(points - 1, lineRenderer.GetPosition(0));
    }
}
