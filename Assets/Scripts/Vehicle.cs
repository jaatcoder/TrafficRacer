using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
}
