using UnityEngine;

public class EnemigoSpawner : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public float tiempoInicial = 5f; // Tiempo inicial entre cada spawneo
    public float tiempoMinimo = 1f; // Tiempo mínimo entre cada spawneo
    public float reduccionTiempo = 0.1f; // Cuánto se reduce el tiempo entre cada spawneo

    private float tiempoSiguienteSpawneo;

    void Start()
    {
        tiempoSiguienteSpawneo = tiempoInicial;
    }

    void Update()
    {
        // Contador para manejar la generación periódica de enemigos
        tiempoSiguienteSpawneo -= Time.deltaTime;
        if (tiempoSiguienteSpawneo <= 0)
        {
            SpawnEnemigo();
            tiempoSiguienteSpawneo = Mathf.Max(tiempoMinimo, tiempoSiguienteSpawneo - reduccionTiempo);
        }
    }

    // Método para generar un nuevo enemigo
    void SpawnEnemigo()
    {
        // spawneamos en una posicion aleatoria dentro de la pantalla
        Vector3 posicionAleatoria = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0));
        posicionAleatoria.z = 0; // Asegurarse de que el enemigo esté en el plano XY
        Instantiate(enemigoPrefab, posicionAleatoria, Quaternion.identity);
    }
}
