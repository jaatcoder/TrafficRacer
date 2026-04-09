using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private float offSet = -5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerCarTransform == null)
        {
            return;
        }

        Vector3 cameraPos = transform.position;
        cameraPos.z = playerCarTransform.position.z + offSet;
        transform.position = cameraPos;
    }

    public void SetTarget(Transform playerTransform)
    {
        playerCarTransform = playerTransform;
    }
}
