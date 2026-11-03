using UnityEngine;

public class AudioControls : MonoBehaviour
{
    public static AudioControls instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource soundSource;

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip gameOverSound;
    public AudioClip backgroundMusic;

    void Awake()
    {
        // Allows other scripts call this audio controller
        instance = this;
    }

    void Start()
    {
        // Loop the background music while the Timmy is running
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayJump()
    {
        soundSource.PlayOneShot(jumpSound); 
    }

    public void PlayDash()
    {
        soundSource.PlayOneShot(dashSound);
    }

    public void PlayGameOver()
    {
        // Replace the regular music with the one-time game over sound.
        musicSource.Stop();
        musicSource.clip = gameOverSound;
        musicSource.loop = false;
        musicSource.Play();
    }
}

//used AI to implement PlayOneShot.
//I learned that this allows for sound effects overlap.