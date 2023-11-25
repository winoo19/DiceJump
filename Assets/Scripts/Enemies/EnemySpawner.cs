using UnityEngine;

public class EnemigoSpawner : MonoBehaviour
{   
    public GameObject[] enemyPrefabs; // Lista de prefabs de enemigos
    public GameObject spawnEffectPrefab; // Prefab del efecto de aparición

    private int waveCount = 0; // Contador de oleadas
    private int enemiesPerWave = 3; // Cantidad de enemigos por oleada inicial
    private float initialSpawnDelay = 10f; // Tiempo inicial entre oleadas
    private float spawnDelayDecrease = 0.05f; // Reducción de tiempo entre oleadas
    private float minSpawnDelay = 5f; // Tiempo mínimo entre oleadas

    private float timeOfNextWave;
    private bool isSpawningWave = false;
    private GameObject[] currentSpawnEffects; // Lista de efectos de aparición de enemigos

    private BoxCollider2D gameBorderCollider; // Limits of the game

    private void Start()
    {   
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
        timeOfNextWave = 0f; // Comenzar con la primera oleada inmediatamente
    }

    private void Update()
    {
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
                                            0);
            randomPos.z = 0;

            currentSpawnEffects[i] = Instantiate(spawnEffectPrefab, randomPos, Quaternion.identity);
        }

        StartCoroutine(SpawnEnemiesWithDelay());
        UpdateWaveProperties();
    }

    private System.Collections.IEnumerator SpawnEnemiesWithDelay()
    {
        yield return new WaitForSeconds(1f); // Esperar un segundo antes de spawneando los enemigos

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

        if (waveCount % 5 == 0) // Cada 5 oleadas aumenta la cantidad de enemigos por oleada
        {
            enemiesPerWave += 1;
        }

        initialSpawnDelay = Mathf.Max(initialSpawnDelay - spawnDelayDecrease, minSpawnDelay); // Reduce el tiempo entre oleadas
        timeOfNextWave = initialSpawnDelay;
    }
}