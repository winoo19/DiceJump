using UnityEngine;
using System.Collections.Generic;

public class DeadEffectTriangleMovement : MonoBehaviour
{
    private float moveSpeed = 5f; // Speed of the triangles
    public float targetRotationSpeed = 30f; // Rotation speed of the triangles
    public float maxRotationAngle = 45f; // Maximum rotation angle of the triangles

    private List<Transform> triangles = new List<Transform>(); // List of child triangles
    private List<Vector3> moveDirections = new List<Vector3>(); // List of initial random directions of the triangles
    private float timer = 0f; // Timer to destroy the GameObject
    private float selfDestructTime = 0.5f; // Time to destroy the GameObject

    void Start()
    {
        // Get all the triangles and store them in a list
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
            Destroy(gameObject);
        }

        for (int i = 0; i < triangles.Count; i++)
        {
            MoveAndRotateTriangle(triangles[i], moveDirections[i]);
        }

        moveSpeed -= moveSpeed * Time.deltaTime/selfDestructTime;
    }

    // Move and rotate each triangle in the direction of the initial random direction
    void MoveAndRotateTriangle(Transform triangle, Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Rotate towards the initial random direction (smoothly)
        triangle.rotation = Quaternion.RotateTowards(triangle.rotation, targetRotation, targetRotationSpeed * Time.deltaTime);

        // Move towards the initial random direction
        triangle.Translate(moveDirection * Time.deltaTime * moveSpeed);
    }
}
