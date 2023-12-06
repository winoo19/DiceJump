using UnityEngine;

public class EnemyType4 : Enemy
{

    protected override void OnStart()
    {
        // change bullet frequency
        bulletFrequency = 0.15f;
    }


    protected override void Move()
    {
        // Move the enemy
        moveDirection = Vector3.zero;

        // Rotate the enemy
        transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
    }

    protected override void Shoot()
    {
        // It will shot in the direction it is facing
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<bulletMovement>().SetDirection(transform.up);

        // Shoot another bullet in the opposite direction
        GameObject bullet2 = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet2.GetComponent<bulletMovement>().SetDirection(-transform.up);
    }
}
