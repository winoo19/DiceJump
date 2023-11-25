using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceMovement : MonoBehaviour
{

    private float speed; // Speed of the dice
    private float rotationSpeed = 0.05f; // Speed of the dice rotation

    private BoxCollider2D gameBorderCollider; // Limits of the game

    // Start is called before the first frame update
    void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the dice upwards very slowly
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Rotate the dice 
        transform.Rotate(0, 0, rotationSpeed);

        // If the dice is outside the game limits, destroy it
        checkOutsideLimits();
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    private void checkOutsideLimits()
    {
        // Take the bounds and check if the dice is outside them (with a little margin)
        if (transform.position.x < gameBorderCollider.bounds.min.x - 1 ||
            transform.position.x > gameBorderCollider.bounds.max.x + 1 ||
            transform.position.y < gameBorderCollider.bounds.min.y - 1 ||
            transform.position.y > gameBorderCollider.bounds.max.y + 1)
        {
            Destroy(gameObject);
        }
    }


}
