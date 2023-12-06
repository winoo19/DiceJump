using UnityEngine;

public class MusicController2 : MonoBehaviour
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

    void Update()
    {
        // Si la música se acaba, la reproducimos de nuevo
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
