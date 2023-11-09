using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 2f; // Speed of the rotation
    public Vector3 startPoint = Vector3.zero;
    public Vector3 endPoint = new Vector3(0f, 4f, 0f);
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;
    public float scaleRate = 1f;

    private bool isJumping = false;
    private float jumpTimer = 0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isJumping)
        {
            Jump();
        }
    }

    private void Jump()
    {
        isJumping = true;
        jumpTimer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJumping)
        {
            JumpAnimation();
        }
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
            float scale = 1f + yPos * scaleRate;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            isJumping = false;
            startPoint = endPoint;
            endPoint = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0f);
            transform.rotation = Quaternion.identity;
        }
    }
}

