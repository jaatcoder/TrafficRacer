using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform PlayercarTransform;
    private Transform cameraPointTransform;
    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayercarTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cameraPointTransform = PlayercarTransform.Find("CameraPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(PlayercarTransform);
        transform.position = Vector3.SmoothDamp(transform.position,cameraPointTransform.position,ref velocity,5f * Time.deltaTime );
    }
}
