using UnityEngine;

public class MusicController2 : MonoBehaviour
{
    public AudioClip musicClip; // Asegúrate de asignar tu archivo de música a esta variable en el Inspector
    private AudioSource musicSource;

    public AudioClip gameOverClip; // Asegúrate de asignar tu archivo de música a esta variable en el Inspector

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true; // Para hacer que la música se reproduzca en bucle
        musicSource.Play();
    }

    void Update()
    {
        // Si la música se acaba (es porque esta sonand GameOver), reproducimos la música de nuevo
        if (!musicSource.isPlaying)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true; // Para hacer que la música se reproduzca en bucle
            musicSource.Play();
        }

    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayGameOverSound()
    {
        musicSource.Stop();
        musicSource.clip = gameOverClip;
        musicSource.loop = false;
        musicSource.Play();

    }

}
