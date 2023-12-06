using UnityEngine;


public class GameManager : MonoBehaviour
{
    private Player player;
    public GameObject playerPrefab;
    public CameraVibration cameraVibration; // Reference to the CameraVibration script  
    public GameObject floatingMenu; // Reference to the floating menu

    public GameObject deadEffectTrianglePrefab; // Prefab of the dead effect triangle
    public float poundRadius = 1.5f;

    private int lives = 3;
    private int score = 0;
    private int highScore = 0;
    private float time = 0;

    public enum GameState
    {
        Playing,
        StandBy
    }
    public static GameState gameState = GameState.StandBy;

    public GameObject heartsPrefab;
    public GameObject timerObject;
    private TMPro.TextMeshProUGUI timerText;
    public GameObject scoreObject;
    private TMPro.TextMeshProUGUI scoreText;
    public GameObject highScoreObject;
    private TMPro.TextMeshProUGUI highScoreText;

    private float invencibilityFrames = 1.5f;
    private float invencibilityFramesCounter = 0;

    public GameObject colorBackground; // To blink the background when the player lands

    private void Start()
    {
        player = FindObjectOfType<Player>();
        timerText = timerObject.GetComponent<TMPro.TextMeshProUGUI>();
        scoreText = scoreObject.GetComponent<TMPro.TextMeshProUGUI>();
        highScoreText = highScoreObject.GetComponent<TMPro.TextMeshProUGUI>();

        scoreText.text = "Score:\n" + score;
        highScoreText.text = "High Score:\n" + highScore;
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
        switch (gameState)
        {
            case GameState.Playing:

                floatingMenu.SetActive(false);
                if (IsOnEnemy())
                {
                    if (invencibilityFramesCounter <= 0)
                    {
                        lives--;
                        StartCoroutine(Blink()); // Blink the player while the invencibility frames are active
                        invencibilityFramesCounter = invencibilityFrames;
                        if (lives <= 0)
                        {
                            Restart();
                        }
                        UpdateHearts();
                    }
                }
                invencibilityFramesCounter -= Time.deltaTime;
                time += Time.deltaTime;
                timerText.text = "Time:\n" + time.ToString("F1");

                break;

            case GameState.StandBy:
                floatingMenu.SetActive(true);
                break;
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
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        // Destroy all enemies
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        // Reset the player
        player.Reset();

        // Update the high score
        highScore = Mathf.Max(highScore, score);
        highScoreText.text = "High Score:\n" + highScore;

        // Reset the score
        score = 0;
        scoreText.text = "Score:\n" + score;

        // Reset the timer
        time = 0;

        // Reset the lives
        lives = 3;

        // Reset the enemy spawner
        EnemigoSpawner enemySpawner = FindObjectOfType<EnemigoSpawner>();
        enemySpawner.ResetWaveProperties();

        // Stop music
        FindObjectOfType<MusicController2>().StopMusic();

        // Change the game state
        gameState = GameState.StandBy;
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
            if (collider.CompareTag("Enemy") || collider.CompareTag("Bullet"))
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
            if (collider.CompareTag("Enemy") || collider.CompareTag("Bullet"))
            {
                Vector3 enemyPosition = collider.transform.position;
                // Destroy the GameObject associated with the collider
                Destroy(collider.gameObject);
                if (collider.CompareTag("Enemy"))
                {
                    score++;
                    scoreText.text = "Score:\n" + score;
                    // Spawn a dead effect were the enemy was
                    Instantiate(deadEffectTrianglePrefab, enemyPosition, Quaternion.identity);
                }
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
