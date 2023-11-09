using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    public int puntos = 50; // Número de puntos que formarán el círculo
    public float radioInicial = 1.0f; // Radio inicial del círculo
    private float lineWith = 0.1f; // Grosor de la línea
    // change de color of the line to white
    private Color c1 = Color.white;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = puntos;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWith;
        lineRenderer.endWidth = lineWith;
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;

        ActualizarRadio(radioInicial);
    }

    // Método para actualizar el radio del círculo
    public void ActualizarRadio(float radio)
    {
        for (int i = 0; i < puntos; i++)
        {
            float angulo = i * (2 * Mathf.PI) / puntos;
            float x = Mathf.Sin(angulo) * radio;
            float y = Mathf.Cos(angulo) * radio;
            Vector3 posicion = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, posicion);
        
        }

        // Asegurémonos de que el último punto sea igual al primero para cerrar el círculo
        lineRenderer.SetPosition(puntos - 1, lineRenderer.GetPosition(0));
    }
}
