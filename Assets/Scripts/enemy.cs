// TODO Interrogación antes de que aparezcan
// TODO Tipos de enemigos


using UnityEngine;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{
    protected float movementSpeed = 0.02f;
    protected float bulletFrequency = 5f; // How often the enemy shoots (seconds)

    protected float timeSinceLastBullet = 0f;

    protected Vector3 directionToPlayer;
    protected Transform playerTransform; // Reference to the player's transform

    // Prefabs of the bullets to shoot:
    protected List<GameObject> bulletPrefabs = new List<GameObject>(); 
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

    protected virtual void Update()
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

    protected virtual void FixedUpdate()
    {
        // Move the enemy towards the dice/player
        transform.Translate(directionToPlayer * movementSpeed);

        // Rotate the enemy towards the dice/player
        if (directionToPlayer != Vector3.zero)
        {   
            // Calculate the rotation to look at the dice/player
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }
    }

    // Each type of enemy will have a different Shoot() method
    protected virtual void Shoot()
    {

    }
}
