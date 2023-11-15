using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void OnEnable()
    {
        Player.OnDiceLanded += CheckEnemyPositions;
    }

    private void OnDisable()
    {
        Player.OnDiceLanded -= CheckEnemyPositions;
    }

    private void CheckEnemyPositions()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // Get all Enemy components in the scene
        Player player = FindObjectOfType<Player>(); // Get the Player component in the scene

        foreach (Enemy enemy in enemies)
        {
            // Check if the enemy's position matches the dice's landed position
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 1f) // Adjust the distance as needed
            {   // Añado un retardo antes de eliminarlo
                Debug.Log("Enemy destroyed!");
                Destroy(enemy.gameObject, 0.7f); // Destroy the enemy
            }
        }
    }
}


/*
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        Player.OnDiceLanded += CheckDiceCollision;
    }

    private void OnDisable()
    {
        Player.OnDiceLanded -= CheckDiceCollision;
    }

    private void CheckDiceCollision()
    {
        // Check for enemies under the dice's landing position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {   
                // Wait a bit before destroying the enemy
                Destroy(collider.gameObject, 0.5f);
                // Add any other logic here related to enemy destruction or game updates
                Debug.Log("Enemy destroyed!");
            }
        }
    }
}
*/