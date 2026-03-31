using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI totalDistanceText;
    [SerializeField] TextMeshProUGUI maximumSpeedText;
    [SerializeField] CarController carController;
    [SerializeField] Transform carTransform;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject speedIcon;
    [SerializeField] GameObject distanceIcon;
    [SerializeField] GameObject scoreIcon;
    private float speed = 0f;
    private float distance = 0f;
    private float score = 0f; 
    private float maximumSpeed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
        speedIcon.SetActive(true);
        distanceIcon.SetActive(true);
        scoreIcon.SetActive(true);
        Time.timeScale = 1f;

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
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        totalScoreText.text = score.ToString("0"+"pts");
        totalDistanceText.text = distance.ToString("0.00" + "km");
        maximumSpeedText.text = speed.ToString("0" + "km/h");

        speedIcon.SetActive(false);
        distanceIcon.SetActive(false);
        scoreIcon.SetActive(false);

    }
    void MaximumSpeed()
    {
        float currentSpeed = carController.CarSpeed();
        if(currentSpeed > maximumSpeed)
        {
            maximumSpeed = currentSpeed;
        }
        maximumSpeedText.text = maximumSpeed.ToString("0"+"km/h");
    }

    public void TryAgain()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }
}
