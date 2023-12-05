using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip musicClip; // Asegúrate de asignar tu archivo de música a esta variable en el Inspector
    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true; // Para hacer que la música se reproduzca en bucle
        musicSource.Play();
    }
}
