using UnityEngine;

public class CarSelection : MonoBehaviour
{
   [SerializeField] GameObject[] cars;
   int currentCarIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowCar(currentCarIndex);
    }
    
    public void NextCar()
    {
        currentCarIndex++;
        if(currentCarIndex>cars.Length-1)
        {
            currentCarIndex = 0;
        }
        ShowCar(currentCarIndex);
    }
    public void PreciousCar()
    {
         currentCarIndex--;
        if(currentCarIndex<0)
        {
            currentCarIndex = cars.Length-1;
        }
        ShowCar(currentCarIndex);
    }
    void ShowCar(int index)
    {
        for( int i = 0  ; i < cars.Length ; i++)
        {
            cars[i].SetActive(i==currentCarIndex);
        }
    }
}
