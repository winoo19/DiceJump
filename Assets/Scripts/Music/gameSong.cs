using UnityEngine;

public class MusicController2 : MonoBehaviour
{
    public AudioClip musicClip; // Music file for the game (StayInsideMe.mp3)
    private AudioSource musicSource; // AudioSource component of the game object
    public AudioClip gameOverClip; // Music file for the game over (GameOver.mp3)

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
