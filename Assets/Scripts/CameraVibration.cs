using UnityEngine;

public class CameraVibration : MonoBehaviour
{
    [SerializeField] private float vibrationIntensity = 0.1f;
    [SerializeField] private float vibrationDuration = 0.2f;

    private bool isVibrating = false;
    private float vibrationTimer = 0f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isVibrating)
        {
            if (vibrationTimer > 0)
            {
                // Generar una vibración aleatoria en la posición de la cámara
                Vector3 vibration = Random.insideUnitSphere * vibrationIntensity;
                transform.localPosition += vibration;

                // Reduce el temporizador de la vibración
                vibrationTimer -= Time.deltaTime;
            }
            else
            {
                // La vibración ha terminado, restablecer la posición original de la cámara
                transform.localPosition = originalPosition;
                isVibrating = false;
            }
        }
    }

    public void TriggerCameraVibration()
    {
        // Comenzar la vibración de la cámara al recibir el evento
        isVibrating = true;
        vibrationTimer = vibrationDuration;
    }
}
