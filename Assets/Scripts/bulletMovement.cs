using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bulletMovement : MonoBehaviour
{
    private float bulletMovementSpeed = 0.05f;
    private Vector3 direction;

    public void SetDirection(Vector3 direction)
    {
        // Set the direction of the bullet
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        // Move the bullet in the direction it was set to
        transform.Translate(direction * bulletMovementSpeed);

        if (transform.position.x > 10 || transform.position.x < -10 || transform.position.y > 10 || transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
