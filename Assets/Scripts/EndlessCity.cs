using UnityEngine;

public class EndlessCity : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private Transform cityTransform;
    [SerializeField] private float halfLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCarTransform.position.z>transform.position.z + halfLength + 15f)
        {
            transform.position = new Vector3(0, 0, cityTransform.position.z + halfLength * 2 );
        }
    }
}
