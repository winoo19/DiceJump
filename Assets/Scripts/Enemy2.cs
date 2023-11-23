using UnityEngine;

public class EnemyType2 : Enemy
{   
    protected override Vector3 GetMoveDirection()
    {
        return Vector3.zero;
    }

    protected override void Move()
    {
        // This enemy doesn't rotate either move towards the dice
        bulletFrequency = 10f;
    }

    protected override void Shoot()
    {
        // Instantiate a bullet and set its direction
        for (int i = 0; i < 15; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(i * 24 * Mathf.Deg2Rad), Mathf.Sin(i * 24 * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<bulletMovement>().SetDirection(direction);
            // change the velocity of the bullet
            bullet.GetComponent<bulletMovement>().SetBulletMovementSpeed(0.05f);
        }
    }
}