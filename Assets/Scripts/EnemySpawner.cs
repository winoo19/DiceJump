using UnityEngine;

public class EnemigoSpawner : MonoBehaviour
{   
    public GameObject[] enemyPrefabs; // Lista de prefabs de enemigos
    public GameObject spawnEffectPrefab; // Prefab del efecto de aparición

    public float spawnDelay = 1f; // Tiempo de demora antes de que aparezca el enemigo

    private bool isSpawningEffect = false;
    private float spawnEffectTimer = 0f;
    private GameObject currentSpawnEffect;

    private float timeOfNextSpawn;
    private Vector3 spawnPosition;

    private void Start()
    {   
        timeOfNextSpawn = spawnDelay;
    }

    private void Update()
    {
        timeOfNextSpawn -= Time.deltaTime;

        if (!isSpawningEffect && timeOfNextSpawn <= 0)
        {
            SpawnEffect();
        }

        if (isSpawningEffect)
        {
            spawnEffectTimer += Time.deltaTime;

            if (spawnEffectTimer >= 1f)
            {
                Destroy(currentSpawnEffect);
                SpawnEnemy();
                isSpawningEffect = false;
            }
        }
    }

    private void SpawnEffect()
    {
        Vector3 randomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0));
        randomPos.z = 0; 

        currentSpawnEffect = Instantiate(spawnEffectPrefab, randomPos, Quaternion.identity);
        isSpawningEffect = true;
        spawnEffectTimer = 0f;
    }

    private void SpawnEnemy()
    {   
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(enemyPrefab, currentSpawnEffect.transform.position, Quaternion.identity);
    }
}
