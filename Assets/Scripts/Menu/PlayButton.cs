using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // there will be 2 sprites(click and non-click)
    public Texture2D clickedSprite; // texture for the clicked button
    public Texture2D nonClickedSprite; // texture for the non-clicked button
    private bool isOnButton = false; // true if the mouse is on the button

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buttonCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (buttonCollider.OverlapPoint(
            Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (!isOnButton)
            {
                isOnButton = true;
                spriteRenderer.sprite = Sprite.Create(clickedSprite,
                    new Rect(0, 0, clickedSprite.width, clickedSprite.height),
                    new Vector2(0.5f, 0.5f));
            }
            // If the player clicks the button, move to the next scene
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
        }
        else if (isOnButton)
        {
            isOnButton = false;
            spriteRenderer.sprite = Sprite.Create(nonClickedSprite,
                new Rect(0, 0, nonClickedSprite.width, nonClickedSprite.height),
                new Vector2(0.5f, 0.5f));
        }
    }
}
