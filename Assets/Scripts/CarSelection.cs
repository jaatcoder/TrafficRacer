using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    private const string CarIndexKey = "CarIndexValue";
    private const string CarNameKey = "SelectedCarName";
    private const string CarIdKey = "SelectedCarId";
    [SerializeField] string gameplaySceneName = "SampleScene";
   [SerializeField] GameObject[] cars;
    [SerializeField] string[] carIds;
    [SerializeField] AudioSource localUiSource;
    [SerializeField] AudioClip localButtonSfx;
   int currentCarIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cars == null || cars.Length == 0)
        {
            return;
        }

        currentCarIndex = Mathf.Clamp(PlayerPrefs.GetInt(CarIndexKey, 0), 0, cars.Length - 1);
        ShowCar(currentCarIndex);
    }
    
    public void NextCar()
    {
        PlayGarageButtonSound();

        currentCarIndex++;
        if(currentCarIndex>cars.Length-1)
        {
            currentCarIndex = 0;
        }
        ShowCar(currentCarIndex);
    }
    public void PreciousCar()
    {
         PlayGarageButtonSound();

         currentCarIndex--;
        if(currentCarIndex<0)
        {
            currentCarIndex = cars.Length-1;
        }
        ShowCar(currentCarIndex);
    }

    // Optional alias in case any button is wired with this common spelling.
    public void PreviousCar()
    {
        PreciousCar();
    }

    public void StartGameFromGarage()
    {
        PlayGarageButtonSound();

        string selectedId = ResolveCarId(currentCarIndex);
        string selectedName = cars != null && currentCarIndex >= 0 && currentCarIndex < cars.Length && cars[currentCarIndex] != null
            ? cars[currentCarIndex].name
            : string.Empty;

        PlayerPrefs.SetInt(CarIndexKey, currentCarIndex);
        PlayerPrefs.SetString(CarNameKey, selectedName);
        PlayerPrefs.SetString(CarIdKey, selectedId);
        PlayerPrefs.Save();

        CarSelectionState.Set(currentCarIndex, selectedId, selectedName);
        Debug.Log($"CarSelection start game with id='{selectedId}', name='{selectedName}', index={currentCarIndex}.");

        SceneManager.LoadScene(gameplaySceneName);
    }

    void ShowCar(int index)
    {
        if (cars == null || cars.Length == 0)
        {
            return;
        }

        for( int i = 0  ; i < cars.Length ; i++)
        {
            cars[i].SetActive(i==currentCarIndex);
        }

        PlayerPrefs.SetInt(CarIndexKey, currentCarIndex);
        PlayerPrefs.SetString(CarNameKey, cars[currentCarIndex].name);
        PlayerPrefs.SetString(CarIdKey, ResolveCarId(currentCarIndex));
        PlayerPrefs.Save();

        CarSelectionState.Set(currentCarIndex, ResolveCarId(currentCarIndex), cars[currentCarIndex].name);
    }

    string ResolveCarId(int index)
    {
        if (carIds != null && index >= 0 && index < carIds.Length && !string.IsNullOrWhiteSpace(carIds[index]))
        {
            return carIds[index].Trim();
        }

        string name = cars[index] != null ? cars[index].name.ToLowerInvariant() : string.Empty;
        if (name.Contains("pick"))
        {
            return "pick-up-truck";
        }

        if (name.Contains("volks"))
        {
            return "volkswagen";
        }

        return "maruti-800";
    }

    void PlayGarageButtonSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonSound();
            return;
        }

        if (localButtonSfx == null)
        {
            return;
        }

        if (localUiSource == null)
        {
            localUiSource = GetComponent<AudioSource>();
            if (localUiSource == null)
            {
                localUiSource = gameObject.AddComponent<AudioSource>();
            }
            localUiSource.playOnAwake = false;
        }

        localUiSource.PlayOneShot(localButtonSfx, 0.7f);
    }
}
