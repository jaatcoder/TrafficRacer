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
        AudioManager.instance.PlayGameMusic();
    }

    // Update is called once per frame
   
}
