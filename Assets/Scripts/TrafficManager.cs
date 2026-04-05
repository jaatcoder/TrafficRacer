using System.Collections;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] Transform[] Lane;
    [SerializeField] GameObject[] TrafficVehicle;
    [SerializeField] CarController CarController;
    [SerializeField] float maxSpawnTime = 60f;
    [SerializeField] float minSpawnTime = 30f;
    private float dynamicTimer=2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       StartCoroutine(SpawnTraffic());
    }

    // Update is called once per frame
    IEnumerator SpawnTraffic()
    {
        yield return new WaitForSeconds(2f);
        while(true)
        {
             
            if (CarController.CarSpeed() > 15)
            {
                dynamicTimer = Random.Range(minSpawnTime, maxSpawnTime) / CarController.CarSpeed();
                SpawnTrafficVehicles();
            }
            yield return new WaitForSeconds(dynamicTimer);
        }
    }
    void SpawnTrafficVehicles()
    {
        int RandomLaneIndex = Random.Range(0, Lane.Length);
        int RandomVehicleIndex = Random.Range(0, TrafficVehicle.Length);

        Instantiate(TrafficVehicle[RandomVehicleIndex], Lane[RandomLaneIndex].position, Quaternion.identity);
    }
}
