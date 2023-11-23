using UnityEngine;

public class EnemyType4 : Enemy
{   

    protected override void Start()
    {   
        // change bullet frequency
        bulletFrequency = 0.5f;


        base.Start();
    }


    protected override void Move()
    {   
        // Move the enemy
        moveDirection = Vector3.zero;
        // transform.Translate(moveDirection * movementSpeed);

        // this one moves towards the dice but rotates very slowly
        // and doesnt' rotate to look at the dice, 
        transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
    }

    protected override void Shoot()
    {
        // It will shot in the direction it is facing
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<bulletMovement>().SetDirection(transform.up);

        // Shot another bullet in the opposite direction
        GameObject bullet2 = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet2.GetComponent<bulletMovement>().SetDirection(-transform.up);
    }
}