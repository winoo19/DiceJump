using UnityEngine;

public class EnemigoSpawner : MonoBehaviour
{   
    // Now instead of spawning a single enemy, there are enemy prefab variants: Enemy, Enemy1 Variant...
    // We can use a list of prefabs to spawn a random enemy variant
    // public GameObject enemyPrefab;
    public GameObject[] enemyPrefabs;

    // public GameObject enemyPrefab;
    public float initialTime = 5f; // Tiempo inicial entre cada spawneo
    public float minTime = 5f; // Tiempo mínimo entre cada spawneo
    public float timeReduction = 0.1f; // Cuánto se reduce el tiempo entre cada spawneo

    private float timeOfNextSpawn;

    private void Start()
    {   
        timeOfNextSpawn = initialTime;
    }

    private void Update()
    {
        // Contador para manejar la generación periódica de enemigos
        timeOfNextSpawn -= Time.deltaTime;
        if (timeOfNextSpawn <= 0)
        {
            SpawnEnemigo();
            timeOfNextSpawn = Mathf.Max(minTime, timeOfNextSpawn - timeReduction);
        }
    }

    // Método para generar un nuevo enemigo
    private void SpawnEnemigo()
    {   
        // Elegimos un enemigo aleatorio de la lista de prefabs
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        // spawneamos en una posicion aleatoria dentro de la pantalla
        Vector3 randomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0));
        randomPos.z = 0; // Asegurarse de que el enemigo esté en el plano XY
        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
    }
}
