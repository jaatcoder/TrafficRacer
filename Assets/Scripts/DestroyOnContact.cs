using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }

        GameObject target = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
        if (target.CompareTag("Player"))
        {
            return;
        }

        bool isTrafficVehicle =
            target.CompareTag("TrafficVehicle") ||
            target.GetComponent<Vehicle>() != null ||
            target.GetComponentInChildren<Vehicle>() != null;

        if (!isTrafficVehicle)
        {
            return;
        }

        Destroy(target);
    }
}
