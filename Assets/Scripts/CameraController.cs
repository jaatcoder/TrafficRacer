using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private Vector3 fallbackOffset = new Vector3(0f, 4.33f, -5.5f);

    private Transform cameraPointTransform;
    private Vector3 velocity = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerCarTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerCarTransform = playerObject.transform;
            }
        }

        ResolveCameraPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCarTransform == null)
        {
            return;
        }

        Vector3 targetPosition = cameraPointTransform != null
            ? cameraPointTransform.position
            : playerCarTransform.TransformPoint(fallbackOffset);

        transform.LookAt(playerCarTransform);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 5f * Time.deltaTime);
    }

    public void SetTarget(Transform playerTransform)
    {
        playerCarTransform = playerTransform;
        ResolveCameraPoint();
    }

    void ResolveCameraPoint()
    {
        cameraPointTransform = null;
        if (playerCarTransform == null)
        {
            return;
        }

        cameraPointTransform = playerCarTransform.Find("CameraPoint");
    }
}
