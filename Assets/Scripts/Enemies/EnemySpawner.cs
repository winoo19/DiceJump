using UnityEngine;
using System.Collections.Generic;

public class EnemigoSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Lista de prefabs de enemigos
    public GameObject spawnEffectPrefab; // Prefab del efecto de aparición

    private int waveCount = 0; // Contador de oleadas
    private int enemiesPerWave; // Cantidad de enemigos por oleada
    private float spawnDelay; // Tiempo entre oleadas

    private float timeOfNextWave;
    private bool isSpawningWave = false;
    private GameObject[] currentSpawnEffects; // Lista de efectos de aparición de enemigos

    private BoxCollider2D gameBorderCollider; // Limits of the game

    public Dictionary<string, float> normalSettings = new Dictionary<string, float>()
    {
        {"initialSpawnDelay", 8f},
        {"spawnDelayDecrease", 0.4f},
        {"minSpawnDelay", 5f},
        {"initialEnemiesPerWave", 2f},
        {"enemyIncrease", 5f} // Add 1 enemy per wave every 5 waves
    };

    public Dictionary<string, float> hardcoreSettings = new Dictionary<string, float>()
    {
        {"initialSpawnDelay", 5f},
        {"spawnDelayDecrease", 0.3f}, // Reduce spawn delay by 0.7 seconds every wave
        {"minSpawnDelay", 4f},
        {"initialEnemiesPerWave", 3f},
        {"enemyIncrease", 3f} // Add 1 enemy per wave every 3 waves
    };

    private Dictionary<GameManager.GameState, Dictionary<string, float>>
        settings = new Dictionary<GameManager.GameState, Dictionary<string, float>>();

    private GameManager.GameState previousGameState;

    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
        timeOfNextWave = 0f; // Comenzar con la primera oleada inmediatamente
        settings.Add(GameManager.GameState.Normal, normalSettings);
        settings.Add(GameManager.GameState.Hardcore, hardcoreSettings);
    }

    private void Update()
    {
        if (GameManager.gameState == GameManager.GameState.StandBy)
        {
            previousGameState = GameManager.GameState.StandBy;
            return;
        }
        if (GameManager.gameState != previousGameState)
        {
            // Reset wave properties when the game state changes
            ResetWaveProperties();
            previousGameState = GameManager.gameState;
        }

        timeOfNextWave -= Time.deltaTime;

        if (!isSpawningWave && timeOfNextWave <= 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        isSpawningWave = true;
        currentSpawnEffects = new GameObject[enemiesPerWave];

        // Generar y almacenar los tres efectos de aparición al mismo tiempo
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // We can only spawn enemies inside the game border
            Vector3 randomPos = new Vector3(Random.Range(gameBorderCollider.bounds.min.x, gameBorderCollider.bounds.max.x),
                                            Random.Range(gameBorderCollider.bounds.min.y, gameBorderCollider.bounds.max.y),
                                            0)
            {
                z = 0
            };

            currentSpawnEffects[i] = Instantiate(spawnEffectPrefab, randomPos, Quaternion.identity);
        }

        StartCoroutine(SpawnEnemiesWithDelay());
        UpdateWaveProperties();
    }

    public void ResetWaveProperties()
    {
        waveCount = 0;
        enemiesPerWave = (int)settings[GameManager.gameState]["initialEnemiesPerWave"];
        spawnDelay = settings[GameManager.gameState]["initialSpawnDelay"];
        timeOfNextWave = 0f;
    }

    private System.Collections.IEnumerator SpawnEnemiesWithDelay()
    {
        yield return new WaitForSeconds(1.5f); // Esperar un segundo antes de spawneando los enemigos

        foreach (GameObject effect in currentSpawnEffects)
        {
            SpawnEnemy(effect);
        }

        foreach (GameObject effect in currentSpawnEffects)
        {
            Destroy(effect); // Eliminar los efectos de aparición después de generar los enemigos
        }

        isSpawningWave = false;
    }

    private void SpawnEnemy(GameObject spawnEffect)
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyPrefab, spawnEffect.transform.position, Quaternion.identity);
    }

    private void UpdateWaveProperties()
    {
        waveCount++;

        if (waveCount % settings[GameManager.gameState]["enemyIncrease"] == 0) // Añadir un enemigo por oleada
        {
            enemiesPerWave += 1;
        }

        spawnDelay = Mathf.Max(
            settings[GameManager.gameState]["minSpawnDelay"],
            spawnDelay - settings[GameManager.gameState]["spawnDelayDecrease"]
        );
        timeOfNextWave = spawnDelay;
    }
}
