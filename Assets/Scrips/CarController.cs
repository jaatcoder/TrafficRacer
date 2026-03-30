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

    private Rigidbody rigidbody;
    private float horizontalInput;
    private float verticalInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = CarcenterOfMass.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MotorForce();
        UpdateWheels();
        GetInput();
        Steering();
        ApplyBrakes();
        PowerSteering();
        Debug.Log("Car Speed: " + CarSpeed() + " mph");

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
            rigidbody.linearDamping = 1f;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            backRightWheelCollider.brakeTorque = 0f;
            backLeftWheelCollider.brakeTorque = 0f;
            rigidbody.linearDamping = 0f;
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
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        transfrom.position = pos;
        transfrom.rotation = rot;
    }
    public float CarSpeed()
    {
        float speed = rigidbody.linearVelocity.magnitude * 2.23693629f;
        return speed;
    }
}
