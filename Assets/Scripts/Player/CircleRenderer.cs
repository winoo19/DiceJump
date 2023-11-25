using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    public int points = 50; // Number of points in the circle
    private float lineWith = 0.05f; // Line width

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWith;
        lineRenderer.endWidth = lineWith;
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

        // Asegurémonos de que el último punto sea igual al primero para cerrar el círculo
        lineRenderer.SetPosition(points - 1, lineRenderer.GetPosition(0));
    }

    // Change the opacity of the line depending on the time since
    // the last jump, to indicate that the player can jump again
    public void ChangeOpacity(float opacity)
    {
        Color c = lineRenderer.startColor;
        c.a = opacity;

        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }
}
