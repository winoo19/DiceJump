using UnityEngine;

public class EnemyType1 : Enemy
{   
    [SerializeField] private float runAwayDistance = 3f; // Distance at which this enemy runs away

    protected override void Move()
    {   
        // This one doesn't move, just rotates
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }

        // If the player is too close, it goes away from it
        // If the player is too close, this enemy runs away from the player
        if (GetVectorToPlayer().magnitude < runAwayDistance)
        {
            // Calculate the direction away from the player
            Vector3 runDirection = transform.position - playerTransform.position;
            transform.Translate(runDirection.normalized * movementSpeed);
        }
    }
}
