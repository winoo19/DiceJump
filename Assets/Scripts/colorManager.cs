using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    private static ColorManager instance;

    private Color startColor = Color.red; // Color inicial
    private Color endColor = Color.blue; // Color final
    private float colorTransitionDuration = 5.0f; // Duración de la transición de color
    private float colorTransitionTime = 0f; // Tiempo actual de transición de color

    private void Start()
    {
        // Establecer el color inicial de los enemigos
        UpdateColorEnemies();
    }

    private void Update()
    {
        // Actualizar el color de los enemigos
        UpdateColorPlayer();
        UpdateColorEnemies();

        // Update color transition time
        colorTransitionTime += Time.deltaTime;

        // Restart the transition if it has ended
        if (colorTransitionTime > colorTransitionDuration)
        {
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
            colorTransitionTime = 0f;
        }

    }

    public void UpdateColorPlayer()
    {
        GameObject player = GameObject.Find("Player"); // Find the GameObject representing the dice
        // Get its spriterenderer component and change its colorgradually
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.color = Color.Lerp(startColor, endColor, colorTransitionTime / colorTransitionDuration);
    }

    public void UpdateColorEnemies()
    {
        // Obtener los enemigos en la escena
        List<GameObject> enemies = GetEnemies();

        // Para cada enemigo, obtener sus círculos hijos y cambiar su color
        foreach (GameObject enemy in enemies)
        {
            // Obtener los círculos hijos para luego obtener sus SpriteRenderer
            List<GameObject> circles = GetCircles(enemy);
            List<SpriteRenderer> circlesSpriteRenderer = GetCirclesSpriteRenderer(circles);

            // Cambiar el color de los círculos
            for (int i = 0; i < circlesSpriteRenderer.Count; i++)
            {
                Color c1 = new Color(startColor.r, startColor.g, startColor.b, startColor.a - (i * 0.15f));
                Color c2 = new Color(endColor.r, endColor.g, endColor.b, endColor.a - (i * 0.15f));
                circlesSpriteRenderer[i].color = Color.Lerp(c1, c2, colorTransitionTime / colorTransitionDuration);
            }

        }

    }

    private List<GameObject> GetEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        return enemies;
    }

    private List<GameObject> GetCircles(GameObject enemy)
    {
        List<GameObject> circles = new List<GameObject>();
        for (int i = 0; i < enemy.transform.childCount; i++)
        {
            circles.Add(enemy.transform.GetChild(i).gameObject);
        }

        return circles;
    }

    private List<SpriteRenderer> GetCirclesSpriteRenderer(List<GameObject> circles)
    {
        List<SpriteRenderer> circlesSpriteRenderer = new List<SpriteRenderer>();
        foreach (GameObject circle in circles)
        {
            circlesSpriteRenderer.Add(circle.GetComponent<SpriteRenderer>());
        }

        return circlesSpriteRenderer;
    }
}
