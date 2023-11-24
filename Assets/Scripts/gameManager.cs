// TODO vidas
// TODO perder
// TODO Puntuacion


using UnityEngine;


public class GameManager : MonoBehaviour
{
    private Player player;
    public GameObject playerPrefab;

    private int lives = 3;

    public GameObject heartsPrefab; // Empty GameObject with the hearts sprites as children

    private float invencibilityFrames = 1f;
    private float invencibilityFramesCounter = 0;

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
            if (invencibilityFramesCounter <= 0)
            {
                lives--;
                UpdateHearts();
                StartCoroutine(Blink()); // Blink the player while the invencibility frames are active
                invencibilityFramesCounter = invencibilityFrames;
                if (lives <= 0)
                {
                    lives = 3;
                    UpdateHearts();
                }
            }
            else
            {
                invencibilityFramesCounter -= Time.deltaTime;
            }
        }
    }

    private System.Collections.IEnumerator Blink()
    {
        float blinkCounter = 0;
        float colorAlpha = 0.5f;
        while (blinkCounter < invencibilityFrames)
        {
            // Get the color of the player, and change gradually and fast its alpha
            Color newColor = player.GetComponent<SpriteRenderer>().color;
            newColor.a = colorAlpha + Mathf.Sin(blinkCounter * 20) * 0.5f;
            player.GetComponent<SpriteRenderer>().color = newColor;

            blinkCounter += Time.deltaTime;
            yield return null; // This statement is required for IEnumerator methods
        }

        // Reset the color of the player
        Color resetColor = player.GetComponent<SpriteRenderer>().color;
        resetColor.a = 1f;
        player.GetComponent<SpriteRenderer>().color = resetColor;

        // Add a return statement to fulfill all code paths
        yield return null;
    }



    private void UpdateHearts()
    {
        for (int i = 0; i < heartsPrefab.transform.childCount; i++)
        {
            if (i < lives)
            {
                heartsPrefab.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                heartsPrefab.transform.GetChild(i).gameObject.SetActive(false);
            }
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
