using UnityEngine;

public class EnemyType3 : Enemy
{
    protected override void Update()
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

    // Now move and rotate
    protected override void FixedUpdate()
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
    
    protected override void Shoot()
    {   
        // For now on this type of enemy just follows
        // the player and doesn't shoot
    }
}
