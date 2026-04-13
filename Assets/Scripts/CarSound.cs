using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] private float minPitch = 0.6f;
    [SerializeField] private float maxPitch = 1.6f;
    [SerializeField] private float maxSpeed = 100f;

    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip brakeSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private CarController carController;

    private AudioSource engineSource;
    private AudioSource sfxSource;
    private bool isBraking;
    private bool warnedMissingEngine;

    void Start()
    {
        AudioListener.pause = false;
        AudioListener.volume = 1f;

        ResolveCarController();
        EnsureDedicatedAudioSources();
        TryAutoAssignFallbackClips();

        Debug.Log($"[CarSound] Init on '{gameObject.name}' (prefab:{gameObject.name}) controller={(carController != null)} engineClip={(engineSound != null)} brakeClip={(brakeSound != null)} hitClip={(hitSound != null)} listenerPause={AudioListener.pause} listenerVolume={AudioListener.volume}");

       
        PlayEngineSound();
        
        
        if (engineSource != null && engineSound != null)
        {
            if (!engineSource.isPlaying)
            {
                Debug.LogError($"[CarSound] Engine sound failed to play on {gameObject.name}. Source enabled: {engineSource.enabled}, muted: {engineSource.mute}");
            }
        }
    }

    void Update()
    {
        if (carController == null)
        {
            ResolveCarController();
        }

        if (carController == null || engineSource == null || sfxSource == null)
        {
            return;
        }

        if (!engineSource.isPlaying && engineSound != null)
        {
            engineSource.Play();
        }

        UpdateEngineSoundPitch();
        HandleBrakeSound();
    }

    private void PlayEngineSound()
    {
        if (engineSource == null || engineSound == null)
        {
            if (!warnedMissingEngine)
            {
                warnedMissingEngine = true;
                Debug.LogWarning("CarSound is missing Engine clip or source on " + gameObject.name + ".");
            }
            return;
        }

        engineSource.clip = engineSound;
        engineSource.loop = true;
        engineSource.volume = 0.4f;
        engineSource.mute = false;
        engineSource.enabled = true;
        
     
        if (!engineSource.isPlaying)
        {
            engineSource.time = 0f; 
            engineSource.Play();
           
            if (!engineSource.isPlaying)
            {
                Debug.LogError($"[CarSound] Failed to start engine sound on {gameObject.name}. Source: {engineSource}, Clip: {engineSound}");
            }
        }
    }

    private void UpdateEngineSoundPitch()
    {
        float speed = carController.CarSpeed();
        float normalizedSpeed = Mathf.Clamp01(speed / Mathf.Max(1f, maxSpeed));
        engineSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
    }

    private void HandleBrakeSound()
    {
        bool shouldBrakeSound = Input.GetKey(KeyCode.Space) && carController.CarSpeed() > 10f && brakeSound != null;

        if (shouldBrakeSound)
        {
            if (!isBraking)
            {
                isBraking = true;
                sfxSource.clip = brakeSound;
                sfxSource.loop = true;
                sfxSource.volume = 1f;
                sfxSource.mute = false;
                sfxSource.enabled = true;
                sfxSource.Play();
            }
        }
        else if (isBraking)
        {
            isBraking = false;
            sfxSource.Stop();
            sfxSource.loop = false;
            sfxSource.clip = null;
        }
    }

    public void PlayHitSound()
    {
        if (sfxSource == null || hitSound == null)
        {
            return;
        }

        if (isBraking)
        {
            isBraking = false;
            sfxSource.Stop();
            sfxSource.loop = false;
            sfxSource.clip = null;
        }

        sfxSource.PlayOneShot(hitSound);
    }

    private void EnsureDedicatedAudioSources()
    {
        engineSource = FindOrCreateSource("EngineAudioSource");
        sfxSource = FindOrCreateSource("SfxAudioSource");

        if (engineSource == null)
        {
            Debug.LogError($"[CarSound] Failed to create engine source on {gameObject.name}");
        }
        if (sfxSource == null)
        {
            Debug.LogError($"[CarSound] Failed to create SFX source on {gameObject.name}");
        }

        ConfigureSourceDefaults(engineSource);
        ConfigureSourceDefaults(sfxSource);
    }

    private AudioSource FindOrCreateSource(string childName)
    {
        Transform child = transform.Find(childName);
        if (child == null)
        {
            GameObject childObject = new GameObject(childName);
            child = childObject.transform;
            child.SetParent(transform, false);
            child.localPosition = Vector3.zero;
            
            if (child == null)
            {
                Debug.LogError($"[CarSound] Failed to create child '{childName}' on {gameObject.name}");
                return null;
            }
        }

        AudioSource source = child.GetComponent<AudioSource>();
        if (source == null)
        {
            source = child.gameObject.AddComponent<AudioSource>();
            if (source == null)
            {
                Debug.LogError($"[CarSound] Failed to add AudioSource component to '{childName}' on {gameObject.name}");
                return null;
            }
        }

        return source;
    }

    private void ConfigureSourceDefaults(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.mute = false;
        source.enabled = true;
        source.ignoreListenerPause = true;
        source.bypassListenerEffects = true;
        source.bypassReverbZones = true;
        source.dopplerLevel = 0f;
        source.volume = Mathf.Max(0.7f, source.volume);
    }

    public void ConfigureClips(AudioClip engine, AudioClip brake, AudioClip hit)
    {
        bool changed = false;
        
        if (engine != null && engineSound != engine)
        {
            engineSound = engine;
            changed = true;
        }

        if (brake != null && brakeSound != brake)
        {
            brakeSound = brake;
            changed = true;
        }

        if (hit != null && hitSound != hit)
        {
            hitSound = hit;
            changed = true;
        }

        // If clips were updated, restart the engine sound
        if (changed && engineSource != null)
        {
            engineSource.Stop();
            PlayEngineSound();
        }
    }

    private void ResolveCarController()
    {
        carController = GetComponent<CarController>();
        if (carController == null)
        {
            carController = GetComponentInParent<CarController>();
        }
        if (carController == null)
        {
            carController = GetComponentInChildren<CarController>();
        }
    }

    private void TryAutoAssignFallbackClips()
    {
        if (engineSound == null)
        {
            engineSound = FindClipByName("MarutiCarEngineMusic");
        }

        if (brakeSound == null)
        {
            brakeSound = FindClipByName("SkidBreakMusic");
        }

        if (hitSound == null)
        {
            hitSound = FindClipByName("CarCrashSound");
        }
    }

    private AudioClip FindClipByName(string clipName)
    {
        AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip clip = clips[i];
            if (clip != null && clip.name == clipName)
            {
                return clip;
            }
        }

        return null;
    }

}
