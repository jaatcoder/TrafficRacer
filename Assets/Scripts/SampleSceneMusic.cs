using UnityEngine;

public class SampleSceneMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

  
    void Start()
    {
        AudioSource localSource = GetComponent<AudioSource>();
        if(localSource != null)
        {
            localSource.Stop();
            localSource.playOnAwake = false;
            localSource.enabled = false;
        }

        if (AudioManager.instance == null)
        {
            Debug.LogWarning("AudioManager instance not found in scene. Game music could not start.");
            return;
        }

        AudioManager.instance.PlayGameMusic();
    }

    // Update is called once per frame
   
}
