
using UnityEngine;
using System.Collections.Generic;

// Effect of the warning sign where an enemy will spawn
public class WarningEffect : MonoBehaviour
{
    public const float fadeSpeed = 0.7f; // How fast the effect fades
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(); // List of the spriterenderers of the children
    private float currentAlpha = 1.0f; // Current alpha of the effec
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
