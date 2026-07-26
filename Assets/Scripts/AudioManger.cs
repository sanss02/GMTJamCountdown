using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource warningSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip sfxShoot;
    [SerializeField] private AudioClip sfxTargetDestroyed;
    [SerializeField] private AudioClip sfxFinalSeconds;
    [SerializeField] private AudioClip sfxClickButton;
    [SerializeField] private AudioClip sfxPlayerExplosion;

    void Awake()
    {
        // Configuración clásica del Singleton
        if (Instance == null)
        {
            Instance = this;
            //Hace que el audio no se destruya al cambiar de escena, para música de fondo
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackgroundMusic(backgroundMusic);
    }

    void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return; //CAMBIAR CUANDO SE TENGA PISTA DE AUDIO

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    //Creo métodos públicos para reproducir los efectos de sonido desde otros scripts
    public void PlaySFXShoot()
    {
        if (sfxSource != null && sfxShoot != null)
        {
            sfxSource.PlayOneShot(sfxShoot);
        }
    }
    public void PlaySFXTargetDestroyed()
    {
        if (sfxSource != null && sfxTargetDestroyed != null)
        {
            sfxSource.PlayOneShot(sfxTargetDestroyed);
        }
    }

    public void PlaySFXClickButton()
    {
        if (sfxSource != null && sfxClickButton != null)
        {
            sfxSource.PlayOneShot(sfxClickButton);
        }
    }

    public void PlaySFXPlayerExplosion()
    {
        if (sfxSource != null && sfxPlayerExplosion != null)
        {
            sfxSource.PlayOneShot(sfxPlayerExplosion);
        }
    }
    
    public void PlaySFXFinalSeconds()
    {
        if (warningSource == null || sfxFinalSeconds == null) return;

        warningSource.clip = sfxFinalSeconds;
        warningSource.time = 0f; // por si se estaba reproduciendo, reinicia desde el principio
        warningSource.Play();
    }

    public void StopSFXFinalSeconds()
    {
        if (warningSource != null) warningSource.Stop();
    }
}