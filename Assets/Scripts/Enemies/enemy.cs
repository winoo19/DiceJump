using UnityEngine;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{

    // Hi
    protected float movementSpeed = 0.02f;
    protected float bulletFrequency = 5f; // How often the enemy shoots (seconds)

    protected float timeSinceLastBullet;

    protected Vector3 moveDirection;
    protected Transform playerTransform; // Reference to the player's transform
    protected float minDistanceToPlayer = 2f;

    public GameObject bulletPrefab; // Prefab of the bullet to shoot

    protected virtual void Start()
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
        OnStart();
        timeSinceLastBullet = bulletFrequency;
    }

    protected virtual void OnStart()
    {
        // This method is called when the enemy is instantiated
    }

    protected virtual void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction towards the dice/player
            moveDirection = GetMoveDirection();
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
        Move();
    }

    protected Vector3 GetVectorToPlayer()
    {
        return playerTransform.position - transform.position;
    }

    protected virtual Vector3 GetMoveDirection()
    {
        // By default move towards the dice/player
        return GetVectorToPlayer().normalized;
    }

    protected virtual void Move()
    {
        // Move the enemy
        transform.Translate(moveDirection * movementSpeed, Space.World);

        // Rotate the enemy in its move direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }
    }


    protected virtual void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<bulletMovement>().SetDirection(moveDirection);
    }

}
