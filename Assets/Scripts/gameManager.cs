// TODO vidas
// TODO perder
// TODO Puntuacion


using UnityEngine;


public class GameManager : MonoBehaviour
{
    private Player player;
    public GameObject playerPrefab;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        Player.OnDiceLanded += DestroyJumpedEnemies;
    }

    private void OnDisable()
    {
        Player.OnDiceLanded -= DestroyJumpedEnemies;
    }

    private void Update()
    {
        if (IsOnEnemy())
        {
            // Restart();
            Debug.Log("Game Over!");
        }
    }

    private void Restart()
    {
        // Find objects with the Enemy tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Destroy all enemies
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Destroy the player and instantiate a new one
        Destroy(player);
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }

    private bool IsOnEnemy()
    {
        if (player.jumpAnimation != null)
        {
            return false;
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            player.transform.position,
            player.GetComponent<BoxCollider2D>().size,
            0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyJumpedEnemies()
    {
        // Get all colliders within a box around the player's position
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            player.transform.position,
            player.GetComponent<BoxCollider2D>().size,
            0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Destroy the GameObject associated with the collider
                Destroy(collider.gameObject);
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
