using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Movement variables (to tweak so that it feels nice)
    public float rotationSpeed = 10f;
    public float jumpHeight = 1f;
    public float jumpDuration = 0.6f;

    public Vector3 startPoint;
    public Vector3 endPoint;

    private float jumpTimer = 0f; // Time taken since jump started

    private void FixedUpdate()
    {
        // We only start the jump once the start point and end point are set
        if (startPoint != null && endPoint != null)
        {
            JumpAnimation();
        }
    }

    public void StartAnimation(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    private void JumpAnimation()
    {
        transform.Rotate(rotationSpeed, 0, 0);
        jumpTimer += Time.deltaTime;

        if (jumpTimer < jumpDuration)
        {
            float progress = jumpTimer / jumpDuration;
            float yPos = Mathf.Sin(progress * Mathf.PI) * jumpHeight;

            transform.position = Vector3.Lerp(startPoint, endPoint, progress);
            transform.position += new Vector3(0f, yPos, 0f);
            // Adjust scale based on the yPos
            float scale = 1f + yPos;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            // Destroy the object when the animation is done
            Destroy(gameObject);
        }
    }
}

