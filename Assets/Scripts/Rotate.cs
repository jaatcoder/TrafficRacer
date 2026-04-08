using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void FixedUpdate()
    {
        transform.Rotate(0 , 0.2f , 0);
    }
}
