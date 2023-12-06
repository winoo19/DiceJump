using UnityEngine;

public class EnemigoSpawner : MonoBehaviour
{
    private const int initialEnemiesPerWave = 3;
    public GameObject[] enemyPrefabs; // List of enemy prefabs
    private GameObject[] currentSpawnEffects; // Effects before spawning the enemies
    public GameObject spawnEffectPrefab; // Effect when spawning an enemy
    private int waveCount = 0; // Counter of waves
    private int enemiesPerWave = initialEnemiesPerWave; // Number of enemies per wave
    private float initialSpawnDelay = 8f; // Initial time between waves 
    private const float spawnDelayDecrease = 0.5f; // Decrease of time between waves
    private const float minSpawnDelay = 5f; // Minimum time between waves
    private float timeOfNextWave; // Time until the next wave
    private bool isSpawningWave = false; // True if the wave is being spawned
    private BoxCollider2D gameBorderCollider; // Limits of the game

    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
        timeOfNextWave = 0f;
    }

    private void Update()
    {
        if (GameManager.gameState != GameManager.GameState.Playing)
        {
            return;
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

        // Spawn the warning effects at random positions inside the game limits
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(gameBorderCollider.bounds.min.x, gameBorderCollider.bounds.max.x),
                                            Random.Range(gameBorderCollider.bounds.min.y, gameBorderCollider.bounds.max.y),
                                            0);
            randomPos.z = 0;

            currentSpawnEffects[i] = Instantiate(spawnEffectPrefab, randomPos, Quaternion.identity);
        }

        // Spawn the enemies after a delay
        StartCoroutine(SpawnEnemiesWithDelay());
        UpdateWaveProperties();
    }

    public void ResetWaveProperties()
    {
        waveCount = 0;
        enemiesPerWave = initialEnemiesPerWave;
        initialSpawnDelay = 8f;
        timeOfNextWave = 0f;
    }

    private System.Collections.IEnumerator SpawnEnemiesWithDelay()
    {
        yield return new WaitForSeconds(1.5f); // Wait 1.5 seconds before spawning the enemies

        foreach (GameObject effect in currentSpawnEffects)
        {
            SpawnEnemy(effect);
        }

        foreach (GameObject effect in currentSpawnEffects)
        {
            Destroy(effect); // Destroy warning effect after spawning the enemy
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

        if (waveCount % 4 == 0) // Increase the number of enemies per wave every 4 waves
        {
            enemiesPerWave += 1;
        }

        // Decrease the time between waves
        initialSpawnDelay = Mathf.Max(initialSpawnDelay - spawnDelayDecrease, minSpawnDelay);
        timeOfNextWave = initialSpawnDelay;
    }
}
