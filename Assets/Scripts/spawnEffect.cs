using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public float fadeSpeed = 1.5f; // Velocidad de desvanecimiento

    private SpriteRenderer spriteRenderer;
    private float currentAlpha = 1.0f; // Opacidad actual
    private bool isFadingOut = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAlpha = spriteRenderer.color.a; // Obtenemos la opacidad actual
    }

    private void Update()
    {
        if (!isFadingOut)
        {
            // Reducir la opacidad gradualmente
            currentAlpha -= fadeSpeed * Time.deltaTime;
            if (currentAlpha <= 0.0f)
            {
                currentAlpha = 0.0f;
                isFadingOut = true; // Iniciar el desvanecimiento hacia afuera
            }
        }
        else
        {
            // Incrementar la opacidad gradualmente
            currentAlpha += fadeSpeed * Time.deltaTime;
            if (currentAlpha >= 1.0f)
            {
                currentAlpha = 1.0f;
                isFadingOut = false; // Reiniciar el parpadeo
            }
        }

        // Establecer la opacidad actual al componente SpriteRenderer
        Color newColor = spriteRenderer.color;
        newColor.a = currentAlpha;
        spriteRenderer.color = newColor;
    }
}
