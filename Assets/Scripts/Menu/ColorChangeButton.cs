using UnityEngine;
using UnityEngine.UI;

public class ColorChangeButton : MonoBehaviour
{
    private Color startColor = Color.red;
    private Color endColor = Color.blue;
    private float colorTransitionDuration = 5.0f;
    private float colorTransitionTime = 0f;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        Image buttonImage = button.GetComponent<Image>();

        startColor = DecreaseSaturation(startColor, 0.5f);
        endColor = DecreaseSaturation(endColor, 0.5f);

        if (buttonImage != null)
        {
            buttonImage.color = startColor;
        }
        else
        {
            Debug.LogError("No se encontró el componente Image en el botón.");
        }
    }

    void Update()
    {
        if (button != null)
        {
            // Update color of objects in the scene
            colorTransitionTime += Time.deltaTime;

            float t = Mathf.Clamp01(colorTransitionTime / colorTransitionDuration);
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = Color.Lerp(startColor, endColor, t);
            }
        }
        else
        {
            Debug.LogError("No se encontró el componente Button adjunto al GameObject.");
        }

        // Restart the transition if it has ended
        if (colorTransitionTime > colorTransitionDuration)
        {
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
            colorTransitionTime = 0f;
        }
    }
    public Color DecreaseSaturation(Color color, float saturation)
    {
        // Decrease the saturation of a color
        Color.RGBToHSV(color, out float h, out float s, out float v);
        Color newColor = Color.HSVToRGB(h, s * saturation, v);
        return newColor;
    }
}
