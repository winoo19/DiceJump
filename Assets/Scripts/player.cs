// TODO: Cambiar el sprite del dado según el número de la cara (crear los sprites)
// TODO: Enemigos
// TODO: Mejorar animación de salto


using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement variables (to tweak so that it feels nice)
    public float moveSpeed = 0.07f;
    public float jumpForce = 1.2f;
    public float rotationSpeed = 8f;

    public int diceNumber = 1;

    private float timeBetweenJumps = 1f;
    private float timeSinceLastJump = 0f;
    private bool hasJumpedRecently = false; // If the player has jumped recently (meaning that he can't jump again)

    // Current movement variables
    private Vector2 moveDirection; // Current direction of the movement
    private Vector2 jumpDirection; // Direction of the jump
    private float currentVelocity; // Current velocity of the rotation (for the smooth rotation)
    private float mouseAngle; // Angle of the vector from the player to the mouse

    // Components
    private BoxCollider2D gameBorderCollider; // Limits of the game
    private CircleRenderer circleRenderer; // Circle that indicates the radius of the jump
    public GameObject jumpAnimationPrefab; // Jump animationk prefab
    private GameObject jumpAnimation; // Instance of the jump animation
    private SpriteRenderer[] spriteRenderers; // Sprite renderers of the player and its children
    private LineRenderer[] lineRenderers; // Line renderers of the player and its children

    // Initialization
    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
        circleRenderer = GameObject.Find("Circle").GetComponent<CircleRenderer>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        lineRenderers = GetComponentsInChildren<LineRenderer>();
    }

    // Update (once per frame)
    private void Update()
    {
        // We only want the player to move when the jump animation is not playing
        if (jumpAnimation == null)
        {
            if (spriteRenderers[0].enabled == false) // If the jump animation just ended
            {
                // Enable the sprite of the player and all its children
                foreach (SpriteRenderer sr in spriteRenderers)
                {
                    sr.enabled = true;
                }
                foreach (LineRenderer lr in lineRenderers)
                {
                    lr.enabled = true;
                }

                // Update the time of the last jump
                timeSinceLastJump = Time.time;

                // Reroll the dice
                diceNumber = Random.Range(1, 7);

                // Update the radius of the circle renderer
                circleRenderer.UpdateRadius(diceNumber);

                // Change opacity of the circle renderer
                circleRenderer.ChangeOpacity(0.3f);
            }

            // If it can jump, we show it by changing the opacity of the circle
            if (hasJumpedRecently && Time.time - timeSinceLastJump > timeBetweenJumps)
            {
                hasJumpedRecently = false;
                circleRenderer.ChangeOpacity(1f);
            }

            // Player movement with WASD
            moveDirection = GetMoveDirection();

            // Player rotation
            mouseAngle = GetMouseAngle();

            // Player jump
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        // We only want the player to move when the jump animation is not playing
        if (jumpAnimation == null && spriteRenderers[0].enabled == true)
        {
            // Move the player in the correct direction independent of the rotation
            transform.Translate(moveDirection, Space.World);

            // Rotate the player to look at the mouse
            float smoothAngle = Mathf.SmoothDampAngle(
                transform.rotation.eulerAngles.z,
                mouseAngle,
                ref currentVelocity,
                1.0f / rotationSpeed
            );
            transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
    }

    // Get the direction of the movement
    private Vector2 GetMoveDirection()
    {
        Vector2 direction = Vector2.zero;
        // Movement to the right if D is pressed
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
        // Left if A is pressed
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        // Up if W is pressed
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        // Down if S is pressed
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        direction.Normalize(); // Normalize the direction so that the player doesn't move faster diagonally

        direction *= moveSpeed;

        // Check if the player stays inside the game borders
        // If not, the movement will be the maximum possible inside the borders
        if (!IsInsideBorders(direction))
        {
            direction = GetMovementInsideBorders(direction);
        }

        return direction;
    }

    // Get the angle of the vector from the player to the mouse
    private float GetMouseAngle()
    {
        // Position of the mouse in the screen
        Vector3 mousePosition = Input.mousePosition;

        // Convert the position of the mouse to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate vector from the player to the mouse
        Vector2 mouseDirection = new Vector2(
            worldPosition.x - transform.position.x,
            worldPosition.y - transform.position.y
        );

        return Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
    }

    // Play jump animation
    private void Jump()
    {
        float actualTime = Time.time;

        if (actualTime - timeSinceLastJump >= timeBetweenJumps)
        {
            // Change the variable that indicates if the player has jumped recently
            hasJumpedRecently = true;

            // Calculate the direction of the jump
            Vector2 jumpDirection = new Vector2(
                Mathf.Cos(mouseAngle * Mathf.Deg2Rad),
                Mathf.Sin(mouseAngle * Mathf.Deg2Rad)
            );


            jumpDirection *= diceNumber * jumpForce;

            // Check if the player stays inside the game borders
            if (!IsInsideBorders(jumpDirection))
            {
                jumpDirection = GetMovementInsideBorders(jumpDirection);
            }

            // Disable the sprite of the player and all its children
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.enabled = false;
            }
            foreach (LineRenderer lr in lineRenderers)
            {
                lr.enabled = false;
            }

            // Play the jump animation
            jumpAnimation = Instantiate(jumpAnimationPrefab, transform.position, Quaternion.identity);
            Rotator jumpAnimationScript = jumpAnimation.GetComponent<Rotator>();
            jumpAnimationScript.StartAnimation(transform.position, transform.position + (Vector3)jumpDirection);

            // Move player to the end of the jump
            transform.Translate(jumpDirection, Space.World);
        }

    }

    // Check if the movement takes the player outside the game borders
    private bool IsInsideBorders(Vector2 movement)
    {
        return gameBorderCollider.bounds.Contains(transform.position + (Vector3)movement);
    }

    // Get the movement that the player should do to stay inside the borders
    private Vector2 GetMovementInsideBorders(Vector2 movement)
    {
        Vector2 movementInsideBorders = movement;

        // We check what border is the one that the player is trying to cross, and we take the 
        // player to that point in that specific border
        if (transform.position.x + movement.x > gameBorderCollider.bounds.max.x)
        {
            movementInsideBorders.x = gameBorderCollider.bounds.max.x - transform.position.x;
        }
        else if (transform.position.x + movement.x < gameBorderCollider.bounds.min.x)
        {
            movementInsideBorders.x = gameBorderCollider.bounds.min.x - transform.position.x;
        }

        if (transform.position.y + movement.y > gameBorderCollider.bounds.max.y)
        {
            movementInsideBorders.y = gameBorderCollider.bounds.max.y - transform.position.y;
        }
        else if (transform.position.y + movement.y < gameBorderCollider.bounds.min.y)
        {
            movementInsideBorders.y = gameBorderCollider.bounds.min.y - transform.position.y;
        }

        return movementInsideBorders;
    }

}
