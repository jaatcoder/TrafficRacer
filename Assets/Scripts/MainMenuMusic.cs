using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        AudioSource localSource = GetComponent<AudioSource>();
        if(localSource != null)
        {
            localSource.Stop();
            localSource.playOnAwake = false;
            localSource.enabled = false;
        }

        if (AudioManager.instance == null)
        {
            Debug.LogWarning("AudioManager instance not found in scene. Menu music could not start.");
            return;
        }

        AudioManager.instance.PlayMenuMusic();
    }

    // Update is called once per frame

}
