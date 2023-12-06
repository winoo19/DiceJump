using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class CircleRendererOnLand : MonoBehaviour
{
    public int points = 50; // Number of points in the circle
    private float lineWidth = 0.2f; // Line width

    private LineRenderer lineRenderer;

    public float radius = 0f;

    private float disapearanceTime = 8f;

    void Awake()
    {
        radius = 0f;
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
        if (radius > 0)
        {
            lineRenderer.enabled = true;
            // diminish de radius
            UpdateRadius((1-radius));
            radius -= disapearanceTime*Time.deltaTime;

        }
        else
        {
            lineWidth = 0.2f;
            lineRenderer.enabled = false;
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

        // Close the circle
        lineRenderer.SetPosition(points - 1, lineRenderer.GetPosition(0));
    }
}
