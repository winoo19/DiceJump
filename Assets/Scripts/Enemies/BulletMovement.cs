using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class bulletMovement : MonoBehaviour
{
    public float bulletMovementSpeed = 0.1f;
    private Vector3 direction;

    private BoxCollider2D gameBorderCollider; // Limits of the game

    public GameObject spandingWavePrefab; // Prefab of the spanding wave (when the bullet is destroyed)


    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetBulletMovementSpeed(float bulletMovementSpeed)
    {
        this.bulletMovementSpeed = bulletMovementSpeed;
    }

    private void FixedUpdate()
    {
        // Move the bullet in the direction it was set to
        transform.Translate(direction * bulletMovementSpeed);

        // Destroy the bullet if it reaches the border of the game
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        if (transform.position.x < gameBorderCollider.bounds.min.x - 0.4 ||
            transform.position.x > gameBorderCollider.bounds.max.x + 0.4 ||
            transform.position.y < gameBorderCollider.bounds.min.y - 0.4 ||
            transform.position.y > gameBorderCollider.bounds.max.y + 0.4)
        {
            // Spawn the spanding wave and destroy the bullet
            Instantiate(spandingWavePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
