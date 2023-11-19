using UnityEngine;

public class EnemyType1 : Enemy
{
    // This one doesn't move towards the player, it just rotates and shoots
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

    // Now rotate
    protected override void FixedUpdate()
    {
        // Rotate the enemy towards the dice/player
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
        // Instantiate a bullet and set its direction

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletPrefabs.Add(bullet); // Add the bullet to the list of bullets
        bullet.GetComponent<bulletMovement>().SetDirection(directionToPlayer);
    }
}
