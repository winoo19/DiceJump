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
    private int normalHighScore = 0;
    private int hardcorelHighScore = 0;
    private float time = 0;

    public enum GameState
    {
        Normal,
        Hardcore,
        StandBy
    }
    public static GameState gameState = GameState.StandBy;

    public GameObject heartsPrefab;
    public GameObject timerObject;
    private TMPro.TextMeshProUGUI timerText;
    public GameObject scoreObject;
    private TMPro.TextMeshProUGUI scoreText;
    public GameObject normalHighScoreObject;
    private TMPro.TextMeshProUGUI normalHighScoreText;
    public GameObject hardcoreHighScoreObject;
    private TMPro.TextMeshProUGUI hardcoreHighScoreText;

    private float invencibilityFrames = 1.5f;
    private float invencibilityFramesCounter = 0;

    public GameObject colorBackground; // To blink the background when the player lands

    public AudioClip smashSoundClip; // Sound when player is hit
    private AudioSource smashSoundSource;

    private void Start()
    {
        smashSoundSource = gameObject.AddComponent<AudioSource>();
        smashSoundSource.clip = smashSoundClip;
        // Subimos el volumen del sonido
        smashSoundSource.volume = 1f;

        player = FindObjectOfType<Player>();
        timerText = timerObject.GetComponent<TMPro.TextMeshProUGUI>();
        scoreText = scoreObject.GetComponent<TMPro.TextMeshProUGUI>();
        normalHighScoreText = normalHighScoreObject.GetComponent<TMPro.TextMeshProUGUI>();
        hardcoreHighScoreText = hardcoreHighScoreObject.GetComponent<TMPro.TextMeshProUGUI>();

        scoreText.text = "Score:\n" + score;
        normalHighScoreText.text = "Normal\nHigh Score:\n" + normalHighScore;
        hardcoreHighScoreText.text = "Hardcore\nHigh Score:\n" + hardcorelHighScore;
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
        if (gameState != GameState.StandBy)
        {
            if (IsOnEnemy())
            {
                if (invencibilityFramesCounter <= 0)
                {
                    lives--;
                    // Play the sound when the player is hit
                    smashSoundSource.Play();
                    StartCoroutine(Blink()); // Blink the player while the invencibility frames are active
                    invencibilityFramesCounter = invencibilityFrames;
                    if (lives <= 0)
                    {
                        // Play game over sound
                        FindObjectOfType<MusicController2>().StopMusic();
                        FindObjectOfType<MusicController2>().PlayGameOverSound();
                        // Change the game state
                        Restart();
                    }
                    UpdateHearts();
                }
            }
            invencibilityFramesCounter -= Time.deltaTime;
            time += Time.deltaTime;
            timerText.text = "Time:\n" + time.ToString("F1");
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
        if (gameState == GameState.Normal)
        {
            normalHighScore = Mathf.Max(normalHighScore, score);
            normalHighScoreText.text = "Normal\nHigh Score:\n" + normalHighScore;
        }
        else if (gameState == GameState.Hardcore)
        {
            hardcorelHighScore = Mathf.Max(hardcorelHighScore, score);
            hardcoreHighScoreText.text = "Hardcore\nHigh Score:\n" + hardcorelHighScore;
        }

        // Reset the score
        score = 0;
        scoreText.text = "Score:\n" + score;

        // Reset the timer
        time = 0;

        // Reset the lives
        lives = 3;

        // Change the game state
        gameState = GameState.StandBy;
        floatingMenu.SetActive(true);
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

    public void ChangeGameStateNormal()
    {
        gameState = GameState.Normal;
        floatingMenu.SetActive(false);
    }

    public void ChangeGameStateHardcore()
    {
        gameState = GameState.Hardcore;
        floatingMenu.SetActive(false);
    }
}
