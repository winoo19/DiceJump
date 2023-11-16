using UnityEngine;


public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        Player.OnDiceLanded += CheckEnemyCollisions;
    }

    private void OnDisable()
    {
        Player.OnDiceLanded -= CheckEnemyCollisions;
    }

    private void CheckEnemyCollisions()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(FindObjectOfType<Player>().transform.position, new Vector2(1f, 1f), 0f);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.Log("Enemy destroyed!");
                Destroy(enemy.gameObject);
            }
        }
    }
}



// Opcion sin colliders
/*
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
                Destroy(enemy.gameObject); // Destroy the enemy
            }
        }
    }
}
*/
