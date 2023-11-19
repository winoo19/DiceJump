using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class bulletMovement : MonoBehaviour
{
    private float bulletMovementSpeed = 0.05f;
    private Vector3 direction;

    private BoxCollider2D gameBorderCollider; // Limits of the game


    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
    }

    public void SetDirection(Vector3 direction)
    {
        // Set the direction of the bullet
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        // Move the bullet in the direction it was set to
        transform.Translate(direction * bulletMovementSpeed);

        // Destroy the bullet if it reaches the border of the game
        destroyBullet();
    }

    // when the bullet reaches the border of the game, destroy it
    private void destroyBullet()
    {
        if (transform.position.x < gameBorderCollider.bounds.min.x || 
            transform.position.x > gameBorderCollider.bounds.max.x || 
            transform.position.y < gameBorderCollider.bounds.min.y || 
            transform.position.y > gameBorderCollider.bounds.max.y)
        {
            Debug.Log("Bullet Destroyed");
            Destroy(gameObject);
        }
    }


}
