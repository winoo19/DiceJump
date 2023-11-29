using UnityEngine;
using System.Collections.Generic;

public class DeadEffectTriangleMovement : MonoBehaviour
{
    private float moveSpeed = 5f; // Velocidad de movimiento de los triángulos
    public float rotationSpeed = 100f; // Velocidad de rotación de los triángulos
    public float targetRotationSpeed = 30f; // Velocidad objetivo de rotación de los triángulos
    public float maxRotationAngle = 45f; // Máximo ángulo de rotación permitido

    private List<Transform> triangles = new List<Transform>(); // Lista de triángulos hijos
    private List<Vector3> moveDirections = new List<Vector3>(); // Lista de direcciones de movimiento
    private float timer = 0f;
    private float selfDestructTime = 0.5f;

    void Start()
    {
        // Obtener todos los triángulos hijos y sus direcciones aleatorias iniciales
        foreach (Transform child in transform)
        {
            triangles.Add(child);
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            Vector3 randomRotation = new Vector3(Random.Range(-maxRotationAngle, maxRotationAngle), Random.Range(-maxRotationAngle, maxRotationAngle), Random.Range(-maxRotationAngle, maxRotationAngle));
            child.Rotate(randomRotation);
            moveDirections.Add(randomDirection);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= selfDestructTime)
        {   
            Destroy(gameObject); // Destruye el GameObject después de 3 segundos     
        }

        // Mover y rotar cada triángulo en su dirección aleatoria inicial
        for (int i = 0; i < triangles.Count; i++)
        {
            MoveAndRotateTriangle(triangles[i], moveDirections[i]);
        }

        moveSpeed -= moveSpeed * Time.deltaTime/selfDestructTime;
    }

    // Mueve y rota un triángulo en una dirección específica
    void MoveAndRotateTriangle(Transform triangle, Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Suavizar la rotación hacia la dirección de movimiento
        triangle.rotation = Quaternion.RotateTowards(triangle.rotation, targetRotation, targetRotationSpeed * Time.deltaTime);

        // Mover en la dirección inicial aleatoria
        triangle.Translate(moveDirection * Time.deltaTime * moveSpeed);
    }
}
