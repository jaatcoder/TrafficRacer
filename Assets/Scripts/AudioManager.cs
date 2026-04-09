using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static AudioManager instance;
        [SerializeField] AudioClip buttonSfx;
        [SerializeField] AudioClip mainMenuMusic;
        [SerializeField] AudioClip sampleSceneMusic;
        private AudioSource musicSource;
        private AudioSource uiSource;
    public void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
            return;  
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        AudioSource[] source = GetComponents<AudioSource>();
        if (source.Length == 0)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            uiSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            foreach (AudioSource audioSource in source)
            {
        
                audioSource.Stop();
                audioSource.playOnAwake = false;
            }

            foreach (AudioSource audioSource in source)
            {
                if (audioSource.loop || audioSource.clip == mainMenuMusic || audioSource.clip == sampleSceneMusic)
                {
                    musicSource = audioSource;
                    break;
                }
            }

            if (musicSource == null)
            {
                musicSource = source[0];
            }

            foreach (AudioSource audioSource in source)
            {
                if (audioSource != musicSource)
                {
                    uiSource = audioSource;
                    break;
                }
            }

            if (uiSource == null)
            {
                uiSource = musicSource;
            }
        }
        if (musicSource != null)
        {
            musicSource.ignoreListenerPause = true;
        }
        if (uiSource != null)
        {
            uiSource.ignoreListenerPause = true;
            uiSource.playOnAwake = false;
        }
        }
    
    public void StopGameMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    public void PlayMenuMusic()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music AudioSource is missing on AudioManager.");
            return;
        }

        if (mainMenuMusic == null)
        {
            Debug.LogWarning("Main menu music clip is not assigned in AudioManager.");
            return;
        }

        if(musicSource.isPlaying&&musicSource.clip==mainMenuMusic)
        {
            return;
        }
        musicSource.Stop();
        musicSource.clip = mainMenuMusic;
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = .2f;
        musicSource.Play();
    }
     public void PlayGameMusic()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music AudioSource is missing on AudioManager.");
            return;
        }

        if (sampleSceneMusic == null)
        {
            Debug.LogWarning("Game music clip is not assigned in AudioManager.");
            return;
        }

        if(musicSource.isPlaying&&musicSource.clip==sampleSceneMusic)
        {
            return;
        }
        musicSource.Stop();
        musicSource.clip = sampleSceneMusic;
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = .2f;
        musicSource.Play();
    }
    public void PlayButtonSound()
    {
        if (uiSource == null)
        {
            Debug.LogWarning("UI AudioSource is missing on AudioManager.");
            return;
        }

        if (buttonSfx == null)
        {
            Debug.LogWarning("Button SFX clip is not assigned in AudioManager.");
            return;
        }
        uiSource.volume = 0.7f;
        uiSource.PlayOneShot(buttonSfx);
        
    }
}
