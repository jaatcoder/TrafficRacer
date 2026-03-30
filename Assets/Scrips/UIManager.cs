using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] CarController carController;
    [SerializeField] Transform carTransform;
    private float speed = 0f;
    private float distance = 0f;
    private float score = 0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUI();
        DistanceUI();
        scoreUI();
    }
    void SpeedUI()
    {
        speed = carController.CarSpeed();
        speedText.text = speed.ToString("0" + "km/h");
    }
    void DistanceUI()
    {
        distance = carTransform.position.z/ 1000;
        distanceText.text = distance.ToString("0.00" + "km");
    }
    void scoreUI()
    {
        score = distance * 6 *1000;
        ScoreText.text = score.ToString("0" + "pts");
    }
}
