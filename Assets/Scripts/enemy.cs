using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 3f;

    private Transform playerTransform; // Reference to the player's transform

    void Start()
    {
        GameObject player = GameObject.Find("Player"); // Find the GameObject representing the dice
        if (player != null)
        {
            playerTransform = player.transform; // Get the transform component of the dice GameObject
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction towards the dice/player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Move the enemy towards the dice/player
            transform.Translate(directionToPlayer * movementSpeed * Time.deltaTime);
        }
    }
}

