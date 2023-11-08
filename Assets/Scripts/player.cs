// TODO: Cambiar el sprite del dado según el número de la cara (crear los sprites)
// TODO: Hacer animación de salto
// TODO: Círculo de salto
// TODO: Enemigos


using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement variables (to tweak so that it feels nice)
    public float moveSpeed = 0.1f;
    public float jumpForce = 1.2f;
    public float rotationSpeed = 8f;

    public int diceNumber = 1; // Number that the dice currently shows

    private Vector2 moveDirection; // Current direction of the movement
    private Vector2 jumpDirection; // Direction of the jump
    private float currentVelocity; // Current velocity of the rotation (for the smooth rotation)
    private float mouseAngle; // Angle of the vector from the player to the mouse

    // Limits of the game
    private BoxCollider2D gameBorderCollider;

    // Initialization
    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
    }

    // Update (once per frame)
    private void Update()
    {
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

    private void FixedUpdate()
    {
        // Move the player in the correcto direction independent of the rotation
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

    // Move the player to the position of the jump
    private void Jump()
    {
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

        transform.Translate(jumpDirection, Space.World);

        // Reroll the dice
        diceNumber = Random.Range(1, 7);
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
