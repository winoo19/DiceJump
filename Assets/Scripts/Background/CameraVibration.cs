using UnityEngine;

public class CameraVibration : MonoBehaviour
{
    [SerializeField] private float vibrationIntensity = 0.1f;
    [SerializeField] private float vibrationDuration = 0.2f;

    private bool isVibrating = false;
    private float vibrationTimer = 0f;  // Timer for the vibration

    private Vector3 originalPosition; // Original position of the camera

    private void Start()
    {
        originalPosition = transform.localPosition; // Store the original position of the camera
    }

    void Update()
    {
        if (isVibrating)
        {
            if (vibrationTimer > 0)
            {
                // Move the camera to a random position inside a sphere of radius vibrationIntensity
                Vector3 vibration = Random.insideUnitSphere * vibrationIntensity;
                transform.localPosition += vibration;

                vibrationTimer -= Time.deltaTime;
            }
            else
            {
                // Reset the camera position
                transform.localPosition = originalPosition;
                isVibrating = false;
            }
        }
    }

    public void TriggerCameraVibration()
    {
        // Trigger the camera vibration
        isVibrating = true;
        vibrationTimer = vibrationDuration;
    }
}
