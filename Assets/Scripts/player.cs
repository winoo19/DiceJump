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
    private float currentVelocity;
    private float mouseAngle;

    private void Update()
    {
        moveDirection = Vector2.zero;
        // Movimiento a la derecha si se presiona la tecla D
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
        }
        // izquierda
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
        }
        // arriba (en eje Y)
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
        }
        // abajo
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

        transform.position += new Vector3(
            jumpDirection.x * diceNumber,
            jumpDirection.y * diceNumber,
            0
        );

        // Cambiamos el numero de la cara del dado
        diceNumber = Random.Range(1, 7);
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
