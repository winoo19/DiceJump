using UnityEngine;

public class EnemyType2 : Enemy
{
    private int nBullets = 20;
    protected override Vector3 GetMoveDirection()
    {
        return Vector3.zero;
    }

    protected override void OnStart()
    {
        bulletFrequency = 5f;
    }

    protected override void Move()
    {
        // This enemy doesn't rotate nor move towards the dice
    }

    protected override void Shoot()
    {
        // Instantiate a bullet and set its direction
        for (int i = 0; i < nBullets; i++)
        {
            Vector3 direction = new Vector3(
                Mathf.Cos(i * 360 / nBullets * Mathf.Deg2Rad),
                Mathf.Sin(i * 360 / nBullets * Mathf.Deg2Rad),
                0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<bulletMovement>().SetDirection(direction);
            // change the velocity of the bullet
            bullet.GetComponent<bulletMovement>().SetBulletMovementSpeed(0.05f);
        }
    }
}