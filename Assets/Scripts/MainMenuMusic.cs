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
        AudioManager.instance.PlayMenuMusic();
    }

    // Update is called once per frame

}
