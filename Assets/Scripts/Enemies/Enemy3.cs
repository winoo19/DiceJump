using UnityEngine;

public class EnemyType3 : Enemy
{
    // This enemy has the default movement (move towards the enemy and rotate)
    protected override void OnStart()
    {
        movementSpeed = 0.05f;
    }

    protected override void Shoot()
    {
        // This enemy doesn't shoot, just follows the player
    }
}
