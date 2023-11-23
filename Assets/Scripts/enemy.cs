// TODO Interrogación antes de que aparezcan
// TODO Tipos de enemigos


using UnityEngine;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{   
    // AÑADIDO: COLOR ======================================
    // Variables de color
    protected Color startColor = Color.red; // Color inicial
    protected Color endColor = Color.blue; // Color final
    protected float colorTransitionDuration = 5.0f; // Duración de la transición de color
    protected float colorTransitionTime = 0f; // Tiempo actual de transición de color

    // Get the circle childs, to then get their sprite renderer
    protected List<GameObject> circles = new List<GameObject>();
    protected List<SpriteRenderer> circlesSpriteRenderer = new List<SpriteRenderer>();

    // =====================================================


    protected float movementSpeed = 0.02f;
    protected float bulletFrequency = 5f; // How often the enemy shoots (seconds)

    protected float timeSinceLastBullet = 0f;

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

        GetChilds();

        StartChangeColor();
    }


    protected void GetChilds()
    {
        // Get the circle childs, to then get their sprite renderer
        for (int i = 0; i < transform.childCount; i++)
        {
            circles.Add(transform.GetChild(i).gameObject);
            circlesSpriteRenderer.Add(circles[i].GetComponent<SpriteRenderer>());
        }
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

        UpdateColor();

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
        transform.Translate(moveDirection * movementSpeed);

        // Rotate the enemy in its move direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }
    }


    protected virtual void Shoot()
    {
        // By default shoot towards the player
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<bulletMovement>().SetDirection(moveDirection);
    }

    protected virtual void StartChangeColor()
    {
        // Stablish initial color of each circle but each with different brightness
        // We want the first to be the lightest and the last the darkest so we go through
        // the list in reverse order
        startColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        endColor = new Color(endColor.r, endColor.g, endColor.b, 1f);
        for (int i = 0; i < circlesSpriteRenderer.Count; i++)
        {
            circlesSpriteRenderer[i].color = new Color(startColor.r, startColor.g, startColor.b, startColor.a - (i * 0.15f));
        }
    }


    protected virtual void UpdateColor()
    {
        // Update the color of each circle
        for (int i = 0; i < circlesSpriteRenderer.Count; i++)
        {   
            // Change the brightness of the color
            Color c1 = new Color(startColor.r, startColor.g, startColor.b, startColor.a - (i * 0.5f));
            Color c2 = new Color(endColor.r, endColor.g, endColor.b, endColor.a - (i * 0.15f));
            circlesSpriteRenderer[i].color = Color.Lerp(c1, c2, colorTransitionTime / colorTransitionDuration);
        }

        // Update the time of the color transition
        colorTransitionTime += Time.deltaTime;

        // If the transition is over, start again
        if (colorTransitionTime > colorTransitionDuration)
        {
            colorTransitionTime = 0f;
        }
    }
}
