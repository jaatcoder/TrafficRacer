using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;


    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;

    [SerializeField] private Transform CarcenterOfMass;


    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float steeringAngle = 30f;
    [SerializeField] private float breakForce = 1000f;
    [SerializeField] private float initialImpactIgnoreSeconds = 0.35f;
    [SerializeField] private UIManager uiManager;

    private Rigidbody carRigidbody;
    private float horizontalInput;
    private float verticalInput;
    private bool hasGameOverTriggered;
    private float spawnTime;

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnsureInitialized();

        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsReadyToDrive())
        {
            return;
        }

        MotorForce();
        UpdateWheels();
        GetInput();
        Steering();
        ApplyBrakes();
        PowerSteering();
    }
    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }
    void ApplyBrakes()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            frontRightWheelCollider.brakeTorque = breakForce;
            frontLeftWheelCollider.brakeTorque = breakForce;
            backRightWheelCollider.brakeTorque = breakForce;
            backLeftWheelCollider.brakeTorque = breakForce;
            carRigidbody.linearDamping = 1f;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            backRightWheelCollider.brakeTorque = 0f;
            backLeftWheelCollider.brakeTorque = 0f;
            carRigidbody.linearDamping = 0f;
        }
    }
    void MotorForce()
    {
        frontRightWheelCollider.motorTorque = motorForce * verticalInput ;
        frontLeftWheelCollider.motorTorque = motorForce * verticalInput;
    }
    void Steering()
    {
        frontRightWheelCollider.steerAngle = horizontalInput * steeringAngle;
        frontLeftWheelCollider.steerAngle = horizontalInput * steeringAngle;
    }
    void PowerSteering()
    {
        if(horizontalInput==0)
        {
            transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.Euler(0f, 0f, 0f),Time.deltaTime);
        }
    }
    void UpdateWheels()
    {
        RotateWheel(frontRightWheelCollider, frontRightWheelTransform);
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        RotateWheel(backRightWheelCollider, backRightWheelTransform);
        RotateWheel(backLeftWheelCollider, backLeftWheelTransform);
    }
    
    void RotateWheel(WheelCollider wheelCollider, Transform transfrom)
    {
        if (wheelCollider == null || transfrom == null)
        {
            return;
        }

        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        transfrom.position = pos;
        transfrom.rotation = rot;
    }
    public float CarSpeed()
    {
        if (carRigidbody == null)
        {
            return 0f;
        }

        float speed = carRigidbody.linearVelocity.magnitude * 2.23693629f;
        return speed;
    }

    public void ConfigureRuntimeSetup(
        WheelCollider frontRight,
        WheelCollider frontLeft,
        WheelCollider backRight,
        WheelCollider backLeft,
        Transform frontRightTransform,
        Transform frontLeftTransform,
        Transform backRightTransform,
        Transform backLeftTransform,
        Transform centerOfMassTransform)
    {
        frontRightWheelCollider = frontRight;
        frontLeftWheelCollider = frontLeft;
        backRightWheelCollider = backRight;
        backLeftWheelCollider = backLeft;

        frontRightWheelTransform = frontRightTransform;
        frontLeftWheelTransform = frontLeftTransform;
        backRightWheelTransform = backRightTransform;
        backLeftWheelTransform = backLeftTransform;
        CarcenterOfMass = centerOfMassTransform;

        EnsureInitialized();
    }

    void EnsureInitialized()
    {
        if (carRigidbody == null)
        {
            carRigidbody = GetComponent<Rigidbody>();
        }

        if (carRigidbody != null && CarcenterOfMass != null)
        {
            carRigidbody.centerOfMass = CarcenterOfMass.localPosition;
        }
    }

    bool IsReadyToDrive()
    {
        return frontRightWheelCollider != null &&
               frontLeftWheelCollider != null &&
               backRightWheelCollider != null &&
               backLeftWheelCollider != null &&
               frontRightWheelTransform != null &&
               frontLeftWheelTransform != null &&
               backRightWheelTransform != null &&
               backLeftWheelTransform != null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        HandleVehicleImpact(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleVehicleImpact(other);
    }

    private void HandleVehicleImpact(Collider other)
    {
        if (hasGameOverTriggered || other == null)
        {
            return;
        }

        if (Time.time - spawnTime < initialImpactIgnoreSeconds)
        {
            return;
        }

        Transform otherRoot = other.transform.root;
        if (otherRoot == transform.root)
        {
            return;
        }

        if (other.CompareTag("Player") || (otherRoot != null && otherRoot.CompareTag("Player")))
        {
            return;
        }

        bool isTrafficVehicle =
            other.CompareTag("TrafficVehicle") ||
            (otherRoot != null && otherRoot.CompareTag("TrafficVehicle")) ||
            other.GetComponent<Vehicle>() != null ||
            (otherRoot != null && otherRoot.GetComponent<Vehicle>() != null) ||
            other.GetComponentInParent<Vehicle>() != null;

        if (!isTrafficVehicle)
        {
            return;
        }

        hasGameOverTriggered = true;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopGameMusic();
        }

        if (uiManager != null)
        {
            uiManager.GameOver();
        }
        else
        {
            Debug.LogError("UIManager reference is missing on CarController.");
        }
    }
}
