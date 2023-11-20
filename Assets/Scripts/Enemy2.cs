
using UnityEngine;

public class EnemyType2 : Enemy
{
    protected override Vector3 GetMoveDirection()
    {
        return Vector3.zero;
    }

    protected override void Shoot()
    {
        // Instantiate a bullet and set its direction
        for (int i = 0; i < 10; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(i * 36 * Mathf.Deg2Rad), Mathf.Sin(i * 36 * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // bulletPrefabs.Add(bullet); // Add the bullet to the list of bullets
            bullet.GetComponent<bulletMovement>().SetDirection(direction);
            // change the velocity of the bullet
            bullet.GetComponent<bulletMovement>().SetBulletMovementSpeed(0.05f);
        }
    }
}
