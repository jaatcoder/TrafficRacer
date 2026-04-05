using UnityEngine;

public class GameOverPanelAnimation : MonoBehaviour
{
    [SerializeField] RectTransform yourScoreRectransform;
    [SerializeField] RectTransform totalScoreRectransform;
    [SerializeField] RectTransform panelRectransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        yourScoreRectransform.localScale = Vector3.zero;

        LeanTween.scale(yourScoreRectransform, Vector3.one, 0.3f);
    }
}
