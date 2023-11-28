// TODO perder
// TODO Puntuacion


using UnityEngine;


public class GameManager : MonoBehaviour
{
    private Player player;
    public GameObject playerPrefab;
    public CameraVibration cameraVibration; // Reference to the CameraVibration script  
    public float poundRadius = 1.5f;

    private int lives = 3;

    public GameObject heartsPrefab; // Empty GameObject with the hearts sprites as children

    private float invencibilityFrames = 1.5f;
    private float invencibilityFramesCounter = 0;

    public GameObject colorBackground; // To blink the background when the player lands

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        Player.OnDiceLanded += DestroyJumpedEnemies;
        Player.OnDiceLanded += TriggerCameraVibration;
        Player.OnDiceLanded += BlinkBackground;
    }

    private void OnDisable()
    {
        Player.OnDiceLanded -= DestroyJumpedEnemies;
        Player.OnDiceLanded -= TriggerCameraVibration;
        Player.OnDiceLanded -= BlinkBackground;
    }

    private void Update()
    {
        if (IsOnEnemy())
        {
            if (invencibilityFramesCounter <= 0)
            {
                lives--;
                StartCoroutine(Blink()); // Blink the player while the invencibility frames are active
                invencibilityFramesCounter = invencibilityFrames;
                if (lives <= 0)
                {
                    lives = 3;
                }
                UpdateHearts();
            }
        }
        invencibilityFramesCounter -= Time.deltaTime;
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
        // Get all colliders within a circle around the player's position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            player.transform.position,
            poundRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Destroy the GameObject associated with the collider
                Destroy(collider.gameObject);
            }
        }
    }

    private void TriggerCameraVibration()
    {
        if (cameraVibration != null)
        {
            cameraVibration.TriggerCameraVibration();
        }
    }

    private void BlinkBackground()
    {
        StartCoroutine(BlinkBackgroundCoroutine());
    }

    private System.Collections.IEnumerator BlinkBackgroundCoroutine()
    {
        // Get the color of the childs 1 and 2 of the background
        GameObject square1 = colorBackground.transform.GetChild(1).gameObject;
        GameObject square2 = colorBackground.transform.GetChild(2).gameObject;
        Color color = square1.GetComponent<SpriteRenderer>().color;

        // Put the alpha at its highest
        color.a = 1f;

        // Set the new color to the background
        square1.GetComponent<SpriteRenderer>().color = color;
        square2.GetComponent<SpriteRenderer>().color = color;

        // Turn it slowly transparent
        while (color.a > 0.8f)
        {
            color.a -= Time.deltaTime * 5;
            square1.GetComponent<SpriteRenderer>().color = color;
            square2.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }
}
