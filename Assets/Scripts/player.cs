// TODO: Cambiar el sprite del dado según el número de la cara (crear los sprites)
// TODO: Hacer animación de salto
// TODO: Implementar bordes del campo
// TODO: Enemigos


using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables de movimiento
    public float moveSpeed = 0.1f;
    public float jumpForce = 10f;
    public float rotationSpeed = 8f;

    public int diceNumber = 1;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 jumpDirection = Vector2.zero;
    private float currentVelocity;
    private float mouseAngle;

    // Limits of the game
    private BoxCollider2D gameBorderCollider;

    // Initialization
    private void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        moveDirection = Vector2.zero;
        // Movement to the right if D is pressed
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
        }
        // Left if A is pressed
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
        }
        // Up if W is pressed
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
        }
        // Down if S is pressed
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector2.down;
        }

        moveDirection.Normalize();

        moveDirection *= moveSpeed;

        // Rotación del jugador
        mouseAngle = getMouseAngle();

        // si se hace click saltamos
        if (Input.GetMouseButtonDown(0))
        {
            // imprimimos por consola el numero de la cara del dado
            Debug.Log(diceNumber);
            jump();
        }
    }

    private void FixedUpdate()
    {   
        // Check if the player stays inside the game borders
        // If not, the movement will be the maximum possible inside the borders
        if (!isInsideBorders(moveDirection))
        {
            moveDirection = getMovementInsideBorders(moveDirection);
        }

        // Movimiento del jugador independiente de la rotacion
        transform.Translate(moveDirection, Space.World);

        // Rotación del jugador
        float smoothAngle = Mathf.SmoothDampAngle(
            transform.rotation.eulerAngles.z,
            mouseAngle,
            ref currentVelocity,
            1.0f / rotationSpeed
        );
        transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
    }

    private float getMouseAngle()
    {
        // Calculamos la dirección del salto
        // Obtener la posición del clic en píxeles
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del clic a coordenadas del mundo
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 mouseDirection = new Vector2(
            worldPosition.x - transform.position.x,
            worldPosition.y - transform.position.y
        );

        return Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
    }

    // Salto
    private void jump()
    {
        // Calculamos la dirección del salto
        Vector2 jumpDirection = new Vector2(
            Mathf.Cos(mouseAngle * Mathf.Deg2Rad),
            Mathf.Sin(mouseAngle * Mathf.Deg2Rad)
        );


        jumpDirection.x = jumpDirection.x * diceNumber;
        jumpDirection.y = jumpDirection.y * diceNumber;

        // Check if the player stays inside the game borders
        if (!isInsideBorders(jumpDirection))
        {
            jumpDirection = getMovementInsideBorders(jumpDirection);
        }

        // Teletransportamos al jugador a la posición del salto
        transform.Translate(jumpDirection, Space.World);

        // Cambiamos el numero de la cara del dado
        diceNumber = Random.Range(1, 7);
    }

    // Check if the movement takes the player outside the game borders
    private bool isInsideBorders(Vector2 movement)
    {
        return gameBorderCollider.bounds.Contains(transform.position + (Vector3)movement);
    }

    // Get the movement that the player should do to stay inside the borders
    private Vector2 getMovementInsideBorders(Vector2 movement)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colision con " + other.name);
        if (other.tag == "Border")
        {
            Debug.Log("Colision con " + other.name);
        }
    }

}
