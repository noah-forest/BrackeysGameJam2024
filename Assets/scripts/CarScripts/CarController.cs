using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody carBody;
    [SerializeField] WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] Transform[] wheelVisuals = new Transform[4];
    const int FWL = 0, FWR = 1, RWL = 2, RWR = 3;

    [SerializeField] float motorPower;
    [SerializeField] float breakForce;
    [SerializeField] float maxSteeringAngle;
    float steeringAngle;

    float verticalInput;
    float horizontalInput;

    bool footOnBreak;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Gas();
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
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        footOnBreak = Input.GetKey(KeyCode.Space);
    }
    void Steer()
    {
        steeringAngle = horizontalInput * maxSteeringAngle;
        wheels[FWL].steerAngle = steeringAngle;
        wheels[FWR].steerAngle = steeringAngle;
    }
    void Gas()
    {
        wheels[RWL].motorTorque = motorPower * verticalInput;
        wheels[RWR].motorTorque = motorPower * verticalInput;
    }

    void Break()
    {
        wheels[RWL].brakeTorque = footOnBreak ? breakForce : 0;
        wheels[RWR].brakeTorque = footOnBreak ? breakForce : 0;
    }

    void UpdateAllWheelVisuals()
    {
        for (int i = 0; i < wheels.Length; i++)
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
