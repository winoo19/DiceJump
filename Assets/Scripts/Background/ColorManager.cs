using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    private static ColorManager instance;

    private Color startColor = Color.red; 
    private Color endColor = Color.blue;
    private float colorTransitionDuration = 5.0f;
    private float colorTransitionTime = 0f;

    public GameObject background; // Background GameObject (parent of the background squares)

    private void Update()
    {
        // Update color of objects in the scene
        UpdateColorPlayer();
        UpdateColorEnemies();
        UpdateColorBakground();
        UpdateColorDiceAnimation();
        UpdatePlayButton();
        UpdateFloatingMenu();

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

    private void UpdateFloatingMenu()
    {
        GameObject floatingMenu = GameObject.Find("FloatingMenu");
        if (floatingMenu != null)
        {
            SpriteRenderer floatingMenuSpriteRenderer = floatingMenu.GetComponent<SpriteRenderer>();
            floatingMenuSpriteRenderer.color = Color.Lerp(startColor, endColor, colorTransitionTime / colorTransitionDuration);
        }
    }

    public void UpdatePlayButton()
    {
        GameObject playButton = GameObject.Find("Play Button");
        if (playButton != null)
        {
            Color opaqueStartColor = DecreaseSaturation(startColor, 0.5f);
            Color opaqueEndColor = DecreaseSaturation(endColor, 0.5f);
            SpriteRenderer playButtonSpriteRenderer = playButton.GetComponent<SpriteRenderer>();
            playButtonSpriteRenderer.color = Color.Lerp(opaqueStartColor, opaqueEndColor, colorTransitionTime / colorTransitionDuration);
        }
    }

    public void UpdateColorBakground()
    {
        // Get its spriterenderer component (child of the background is a square with a sprite renderer)
        SpriteRenderer backgroundSpriteRenderer = background.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer backgroundSpriteRenderer1 = background.transform.GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer backgroundSpriteRenderer2 = background.transform.GetChild(2).GetComponent<SpriteRenderer>();

        // Get the opposite color of the start and end color (background has a different color than the other objects)
        Color b1 = new Color(endColor.r, endColor.g, endColor.b, 0.2f);
        Color b2 = new Color(startColor.r, startColor.g, startColor.b, 0.2f);

        // Opaque color for the side squares of the background
        Color opaqueStartColor = DecreaseSaturation(startColor, 0.8f);
        Color opaqueEndColor = DecreaseSaturation(endColor, 0.8f);
        Color b11 = new Color(opaqueEndColor.r, opaqueEndColor.g, opaqueEndColor.b, 1f);
        Color b22 = new Color(opaqueStartColor.r, opaqueStartColor.g, opaqueStartColor.b, 1f);

        // Apply the color transition to the background squares
        backgroundSpriteRenderer.color = Color.Lerp(b2, b1, colorTransitionTime / colorTransitionDuration);
        backgroundSpriteRenderer1.color = Color.Lerp(b22, b11, colorTransitionTime / colorTransitionDuration);
        backgroundSpriteRenderer2.color = Color.Lerp(b22, b11, colorTransitionTime / colorTransitionDuration);
    }

    public void UpdateColorPlayer()
    {
        GameObject player = GameObject.Find("Player"); // Find the GameObject representing the dice
        GameObject triangle = GameObject.Find("Triangle"); // Find the GameObject representing the arrow
        // Get its spriterenderer component and change its colorgradually
        if (player != null)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            SpriteRenderer triangleSpriteRenderer = triangle.GetComponent<SpriteRenderer>();

            playerSpriteRenderer.color = Color.Lerp(startColor, endColor, colorTransitionTime / colorTransitionDuration);
            triangleSpriteRenderer.color = Color.Lerp(startColor, endColor, colorTransitionTime / colorTransitionDuration);
        }
    }

    public void UpdateColorEnemies()
    {
        List<GameObject> enemies = GetEnemies();

        foreach (GameObject enemy in enemies)
        {
            // Change color of each child of the enemy (each one with different alpha)
            List<GameObject> circles = GetCircles(enemy);
            List<SpriteRenderer> circlesSpriteRenderer = GetCirclesSpriteRenderer(circles);

            for (int i = 0; i < circlesSpriteRenderer.Count; i++)
            {
                Color c1 = new Color(startColor.r, startColor.g, startColor.b, startColor.a - (i * 0.15f));
                Color c2 = new Color(endColor.r, endColor.g, endColor.b, endColor.a - (i * 0.15f));
                circlesSpriteRenderer[i].color = Color.Lerp(c1, c2, colorTransitionTime / colorTransitionDuration);
            }
        }
    }

    private void UpdateColorDiceAnimation()
    {
        GameObject cubeAnimation = GameObject.FindGameObjectWithTag("CubeAnimation");

        if (cubeAnimation != null)
        {
            Material cubeAnimationMaterial = cubeAnimation.GetComponent<Renderer>().material;
            cubeAnimationMaterial.color = Color.Lerp(startColor, endColor, colorTransitionTime / colorTransitionDuration);
        }
    }

    private List<GameObject> GetEnemies()
    {
        // Get all the enemies in the scene
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        return enemies;
    }

    private List<GameObject> GetCircles(GameObject enemy)
    {
        // Get the circles of the enemy (children of the enemy)
        List<GameObject> circles = new List<GameObject>();
        for (int i = 0; i < enemy.transform.childCount; i++)
        {
            circles.Add(enemy.transform.GetChild(i).gameObject);
        }

        return circles;
    }

    private List<SpriteRenderer> GetCirclesSpriteRenderer(List<GameObject> circles)
    {
        // Get the sprite renderer of each circle
        List<SpriteRenderer> circlesSpriteRenderer = new List<SpriteRenderer>();
        foreach (GameObject circle in circles)
        {
            circlesSpriteRenderer.Add(circle.GetComponent<SpriteRenderer>());
        }

        return circlesSpriteRenderer;
    }

    public Color DecreaseSaturation(Color color, float saturation)
    {
        // Decrease the saturation of a color
        Color.RGBToHSV(color, out float h, out float s, out float v);
        Color newColor = Color.HSVToRGB(h, s * saturation, v);
        return newColor;
    }

}
