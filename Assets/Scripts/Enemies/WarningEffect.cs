// This scripts will be used by each warning sign to change it's oppacity

using UnityEngine;
using System.Collections.Generic;

public class WarningEffect : MonoBehaviour
{
    public const float fadeSpeed = 0.7f; // Velocidad de desvanecimiento

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private float currentAlpha = 1.0f; // Opacidad actual

    private void Start()
    {
        // Take the spriterenderer of each child
        for (int i = 0; i < transform.childCount; i++)
        {
            spriteRenderers.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    private void Update()
    {
        currentAlpha -= fadeSpeed * Time.deltaTime;
        // Go through every spriterenderer and change it's alpha
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            Color newColor = spriteRenderers[i].color;
            newColor.a = currentAlpha;
            spriteRenderers[i].color = newColor;
        }

    }
}
