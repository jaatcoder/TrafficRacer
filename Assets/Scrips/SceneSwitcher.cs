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
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= speed*Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

     IEnumerator FadeOut(string sceneName)
    {
        while(canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed*Time.unscaledDeltaTime;
            yield return null;
        }
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
