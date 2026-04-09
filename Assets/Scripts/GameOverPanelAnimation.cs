using UnityEngine;
using System.Collections;

public class GameOverPanelAnimation : MonoBehaviour
{
    [SerializeField] RectTransform yourScoreRectransform;
    [SerializeField] RectTransform totalScoreRectransform;
    [SerializeField] RectTransform panelRectransform;
    [SerializeField] float scaleDuration = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        yourScoreRectransform.localScale = Vector3.zero;
        StartCoroutine(ScaleIn(yourScoreRectransform, Vector3.zero, Vector3.one, scaleDuration));
    }

    IEnumerator ScaleIn(RectTransform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            target.localScale = Vector3.LerpUnclamped(from, to, t);
            yield return null;
        }

        target.localScale = to;
    }
}
