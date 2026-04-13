using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float speed = 1f;
    void Awake()
    {
        Debug.Log("[SceneSwitcher] Awake called");
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            Debug.Log("[SceneSwitcher] CanvasGroup is null in Awake");
        }
    }

    IEnumerator FadeIn()
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        while(canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= speed*Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

     IEnumerator FadeOut(string sceneName)
    {
        Debug.Log($"[FadeOut] Started for scene: {sceneName}, canvasGroup={canvasGroup}");
        
        if (canvasGroup == null)
        {
            Debug.Log($"[FadeOut] CanvasGroup is null, loading scene immediately: {sceneName}");
            SceneManager.LoadScene(sceneName);
            Debug.Log($"[FadeOut] SceneManager.LoadScene called with: {sceneName}");
            yield break;
        }

        while(canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed*Time.unscaledDeltaTime;
            yield return null;
        }
        
        Debug.Log($"[FadeOut] Fade complete, loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void SceneLoader(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
        
    }

    public void SceneLoaderWithSound(string sceneName)
    {
        Debug.Log($"[SceneLoaderWithSound] Called with sceneName='{sceneName}'");
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonSound();
            Debug.Log($"[SceneLoaderWithSound] Button sound played");
        }
        else
        {
            Debug.LogWarning("[SceneLoaderWithSound] AudioManager instance not found");
        }
        
        StartCoroutine(FadeOut(sceneName));
        Debug.Log($"[SceneLoaderWithSound] FadeOut coroutine started for scene: {sceneName}");
    }

    public void QuitGameWithSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonSound();
        }
        Application.Quit();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
