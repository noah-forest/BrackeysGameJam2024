using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] Rigidbody carBody;
    [SerializeField] BoxCollider mainCollider;
    [SerializeField] WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] Transform[] wheelVisuals = new Transform[4];
    const int FWL = 0, FWR = 1, RWL = 2, RWR = 3, ROOF = 4;

    [SerializeField] float motorPower;
    [SerializeField] float breakForce;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] float turboPower;
    [SerializeField] float unstickForce;
    [SerializeField] GameObject superUnstickPrefab;

    float steeringAngle;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    public bool FootOnBreak { get; private set; }
    public bool FootOnTurbo { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Gas();
        Turbo();
        Steer();
        Break();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateAllWheelVisuals();
    }

    void GetInput()
    {
        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");
        FootOnBreak = Input.GetKey(KeyCode.Space);
        FootOnTurbo = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetMouseButton(0))
        {
            float xPos = Random.Range(mainCollider.bounds.min.x, mainCollider.bounds.max.x);
            float yPos = Random.Range(mainCollider.bounds.min.y, mainCollider.bounds.max.y);
            float zPos = Random.Range(mainCollider.bounds.min.z, mainCollider.bounds.max.z);
            Vector3 forceAxis = Vector3.zero;
            switch (Random.Range(0, 2))
            {
                case 0:
                    forceAxis = Vector3.forward;
                    break; 
                case 1:
                    forceAxis = Vector3.right;
                    break;
                case 2:
                    forceAxis = Vector3.up;
                    break;
            }
            forceAxis *= (Random.value > 0.5 ? 1 : -1);
            carBody.AddForceAtPosition(unstickForce * forceAxis,new Vector3(xPos, yPos, zPos));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(superUnstickPrefab, transform.position,Quaternion.identity);
        }

    }
    void Steer()
    {
        steeringAngle = HorizontalInput * maxSteeringAngle;
        wheels[FWL].steerAngle = steeringAngle;
        wheels[FWR].steerAngle = steeringAngle;
    }
    void Gas()
    {
        //wheels[RWL].motorTorque = motorPower * verticalInput;
        //wheels[RWR].motorTorque = motorPower * verticalInput;
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = motorPower * VerticalInput;
        }
    }

    void Turbo()
    {
        if (!FootOnTurbo) return;
        Vector3 turboTarget = transform.position;
        turboTarget += transform.forward * VerticalInput * 5;
        Vector3 turboForce = transform.forward * turboPower * VerticalInput;
        carBody.AddForceAtPosition(turboForce, turboTarget);
    }

    void Break()
    {
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = FootOnBreak ? breakForce : 0;
        }
        //wheels[RWL].brakeTorque = footOnBreak ? breakForce : 0;
        //wheels[RWR].brakeTorque = footOnBreak ? breakForce : 0;

    }

    void UpdateAllWheelVisuals()
    {
        for (int i = 0; i < wheelVisuals.Length; i++)
        {
            UpdateWheelVisual(wheels[i], wheelVisuals[i]);
        }
    }

    void UpdateWheelVisual(WheelCollider wheel, Transform visual)
    {
        if(!wheel || !visual) return;
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);
        visual.rotation = rot;
        visual.position = pos;
    }
}
