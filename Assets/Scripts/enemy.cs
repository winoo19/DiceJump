// TODO Interrogación antes de que aparezcan
// TODO Tipos de enemigos


using UnityEngine;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{
    private float movementSpeed = 0.02f;
    private float bulletFrequency = 1f; // How often the enemy shoots (seconds)

    private float timeSinceLastBullet = 0f;

    private Vector3 directionToPlayer;

    private Transform playerTransform; // Reference to the player's transform
    public GameObject bulletPrefab; // Prefab of the bullet to shoot

    private void Start()
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

    private void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction towards the dice/player
            directionToPlayer = (playerTransform.position - transform.position).normalized;

        }
        if (timeSinceLastBullet >= bulletFrequency)
        {
            Shoot();
            timeSinceLastBullet = 0f;
        }
        else
        {
            timeSinceLastBullet += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Move the enemy towards the dice/player
        transform.Translate(directionToPlayer * movementSpeed);
    }

    private void Shoot()
    {
        // Instantiate a bullet prefab at the enemy's position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<bulletMovement>().SetDirection(directionToPlayer);
    }
}
