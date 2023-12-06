using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField] private float runAwayDistance = 3f; // Distance at which this enemy runs away

    private Collider2D gameBorderCollider;

    protected override void OnStart()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<Collider2D>();
    }

    protected override void Move()
    {
        // Rotate the enemy in its move direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }
        // If the player is too close, it goes away from it without getting out of bounds
        if (GetVectorToPlayer().magnitude < runAwayDistance)
        {
            // Calculate the direction away from the player
            Vector3 runDirection = (transform.position - playerTransform.position);
            if (!IsInsideBorders(runDirection))
            {
                runDirection = GetMovementInsideBorders(runDirection);
            }
            transform.Translate(runDirection * movementSpeed, Space.World);
        }
    }

    private bool IsInsideBorders(Vector2 movement)
    {
        return gameBorderCollider.bounds.Contains(transform.position + (Vector3)movement);
    }

    // If the movement would get the enemy out of the borders, return the movement that keeps it inside the borders
    private Vector2 GetMovementInsideBorders(Vector2 movement)
    {
        Vector2 movementInsideBorders = movement;

        if (transform.position.x + movement.x > gameBorderCollider.bounds.max.x)
        {
            movementInsideBorders.x = gameBorderCollider.bounds.max.x - transform.position.x;
        }
        else if (transform.position.x + movement.x < gameBorderCollider.bounds.min.x)
        {
            movementInsideBorders.x = gameBorderCollider.bounds.min.x - transform.position.x;
        }

        if (transform.position.y + movement.y > gameBorderCollider.bounds.max.y)
        {
            movementInsideBorders.y = gameBorderCollider.bounds.max.y - transform.position.y;
        }
        else if (transform.position.y + movement.y < gameBorderCollider.bounds.min.y)
        {
            movementInsideBorders.y = gameBorderCollider.bounds.min.y - transform.position.y;
        }

        return movementInsideBorders;
    }
}
