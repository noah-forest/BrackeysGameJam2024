using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarController : MonoBehaviour
{

    [SerializeField] Rigidbody carBody;
    [SerializeField] BoxCollider mainCollider;
    [SerializeField] WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] Transform[] wheelVisuals = new Transform[4];
    const int FWL = 0, FWR = 1, RWL = 2, RWR = 3;


    [SerializeField] float turboPower;
    [SerializeField] float unstickForce;
    [SerializeField] GameObject superUnstickPrefab;

    [SerializeField] Transform[] steerJets;
    [SerializeField] float steerJetPower;

    [Space(20)]
    //[Header("CAR SETUP")]
    [Space(10)]
    [Range(20, 500)]
    public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.
    [Range(10, 500)]
    public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
    [Range(1, 100)]
    public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.
    [Range(1, 100)]
    public int reverseAccelerationMultiplier = 2; // How fast the car can accelerate backwards. 1 is a slow acceleration and 10 is the fastest.
    [Space(10)]
    [Range(10, 90)]
    public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
    [Range(0.1f, 100f)]
    public float steeringSpeed = 0.5f; // How fast the steering wheel turns.
    [Range(0.1f, 100f)]
    public float centeringSpeed = 0.5f; // How fast the steering wheel returns to straight.
    [Space(10)]
    [Range(100, 1000)]
    public int brakeForce = 350; // The strength of the wheel brakes.
    [Range(1, 30)]
    public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
    [Range(1, 30)]
    public int handbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.
    [Range(0, 1)]
    [SerializeField] float flatDrift = 1;
    [Range(0, 10)]
    [SerializeField] float maxDriftTime = 1;
    float timeSpentBreaking;
    [SerializeField] float velocityEffectOnSteerJets = 3;
    [Space(10)]

    [Space(20)]
    //[Header("EFFECTS")]
    [Space(10)]
    //The following variable lets you to set up particle systems in your car

    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;

    [Space(10)]
    // The following trail renderers are used as tire skids when the car loses traction.
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    [Space(20)]
    //[Header("Sounds")]
    [Space(10)]
    //The following variable lets you to set up sounds for your car such as the car engine or tire screech sounds.
    public AudioSource carEngineSound; // This variable stores the sound of the car engine.
    public AudioSource tireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
    float initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

    [HideInInspector]
    public float carSpeed; // Used to store the speed of the car.
    [HideInInspector]
    public bool isDrifting; // Used to know whether the car is drifting or not.
    [HideInInspector]
    public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.

    float driftingAxis;
    float localVelocityZ;
    float localVelocityX;
    bool deceleratingCar;
    float decelTimeStamp;
    float decelInterval = 0.0f;
    bool recoveringTraction;
    float carSoundTimeStamp;
    float carSoundInterval = 0.1f;
    Vector2 xzVel = Vector2.zero;

    [SerializeField]
    [Range(0,3000)]
    float maxDownforce = 100;
    [SerializeField]
    [Range(0, 10)]
    float velocityDownforceScalar = 0.5f;

    struct WheelFrictionInfo
    {
        public WheelFrictionCurve curve;
        public float externumSlip;
    }

    WheelFrictionInfo[] frictionInfo = new WheelFrictionInfo[4];



    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    public float motorAxis;
    public float steeringAxis;
    public bool FootOnBreak { get; private set; }
    public bool FootOnTurbo { get; private set; }
    // Start is called before the first frame update





    void Start()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelCollider wheel = wheels[i];
            frictionInfo[i] = new WheelFrictionInfo();
            frictionInfo[i].curve = new WheelFrictionCurve();
            frictionInfo[i].externumSlip = wheel.sidewaysFriction.extremumSlip;
            frictionInfo[i].curve.extremumValue = wheel.sidewaysFriction.extremumValue;
            frictionInfo[i].curve.asymptoteSlip = wheel.sidewaysFriction.asymptoteSlip;
            frictionInfo[i].curve.asymptoteValue = wheel.sidewaysFriction.asymptoteValue;
            frictionInfo[i].curve.stiffness = wheel.sidewaysFriction.stiffness;
        }

        // We save the initial pitch of the car engine sound.
        if (carEngineSound != null)
        {
            initialCarEngineSoundPitch = carEngineSound.pitch;
        }
    }

    void CarSounds()
    {
        try
        {
            if (carEngineSound != null)
            {
                float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carBody.velocity.magnitude) / 25f);
                carEngineSound.pitch = engineSoundPitch;
            }
            if ((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f))
            {
                if (!tireScreechSound.isPlaying)
                {
                    tireScreechSound.Play();
                }
            }
            else if ((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f))
            {
                tireScreechSound.Stop();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    private void FixedUpdate()
    {
        // We determine the speed of the car.
        carSpeed = (2 * Mathf.PI * wheels[0].radius * wheels[0].rpm * 60) / 1000;
        // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
        localVelocityX = transform.InverseTransformDirection(carBody.velocity).x;
        // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
        localVelocityZ = transform.InverseTransformDirection(carBody.velocity).z;



        if (Time.time >= carSoundTimeStamp)
        {
            CarSounds();
            carSoundTimeStamp = Time.time + carSoundInterval;
        }
        if (deceleratingCar && Time.time >= decelTimeStamp)
        {
            DecelerateCar();
            decelTimeStamp = Time.time + decelInterval;
        }
        if (recoveringTraction)
        {
            RecoverTraction();
        }
        if(FootOnBreak)
        {
            timeSpentBreaking += Time.deltaTime;
        }

        Gas();
        Steer();
        Handbrake();
        Turbo();
        ApplyDownforce();
    }

    void ApplyDownforce()
    {

        carBody.AddForce(0, -carBody.velocity.magnitude * velocityDownforceScalar, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

    }

    void GetInput()
    {
        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");
        FootOnBreak = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RecoverTraction();
        }
        FootOnTurbo = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            timeSpentBreaking = 0;
        }


        UnStick();

    }

    void UnStick()
    {
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
            carBody.AddForceAtPosition(unstickForce * forceAxis, new Vector3(xPos, yPos, zPos));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(superUnstickPrefab, transform.position, Quaternion.identity);
        }
    }

    void Steer()
    {
        //SteerWheel(wheels[FWL]);
        //SteerWheel(wheels[FWR]);
        if (HorizontalInput > 0)
        {
            TurnRight();
        }
        else if (HorizontalInput < 0)
        {
            TurnLeft();
        }
        else if (steeringAxis != 0)
        {
            ResetSteeringAngle();
        }

    }

    public void TurnLeft()
    {

        steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        if (steeringAxis < -1f)
        {
            steeringAxis = -1f;
        }
        var steeringAngle = steeringAxis * maxSteeringAngle;
        //Debug.Log($"[TurnLeft] SteeringAxis: {steeringAxis} : SteeringAngle: {steeringAngle}");
        wheels[FWL].steerAngle = Mathf.Lerp(wheels[FWL].steerAngle, steeringAngle, Time.deltaTime * steeringSpeed * 10);
        wheels[FWR].steerAngle = Mathf.Lerp(wheels[FWR].steerAngle, steeringAngle, Time.deltaTime * steeringSpeed * 10);

        carBody.AddForceAtPosition((steerJetPower + (localVelocityZ * velocityEffectOnSteerJets)) * -steerJets[0].forward, steerJets[0].position);
    }

    //The following method turns the front car wheels to the right. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnRight()
    {
        steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        if (steeringAxis > 1f)
        {
            steeringAxis = 1f;
        }
        var steeringAngle = steeringAxis * maxSteeringAngle;
        wheels[FWL].steerAngle = Mathf.Lerp(wheels[FWL].steerAngle, steeringAngle, steeringSpeed);
        wheels[FWR].steerAngle = Mathf.Lerp(wheels[FWR].steerAngle, steeringAngle, steeringSpeed);

        carBody.AddForceAtPosition((steerJetPower + (localVelocityZ * velocityEffectOnSteerJets)) * -steerJets[1].forward, steerJets[1].position);
    }

    void SteerWheel(WheelCollider wheel)
    {
        steeringAxis = steeringAxis + HorizontalInput * (Time.deltaTime * 10f * steeringSpeed);
        Mathf.Clamp(steeringAxis, -1, 1);
        var steeringAngle = steeringAxis * maxSteeringAngle;
        wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steeringAngle, steeringSpeed);
    }

    // on the steeringSpeed variable.
    public void ResetSteeringAngle()
    {
        if (steeringAxis < 0f)
        {
            steeringAxis = Mathf.Lerp(steeringAxis, 0, (Time.deltaTime * 10f * centeringSpeed));
        }
        else if (steeringAxis > 0f)
        {
            steeringAxis = Mathf.Lerp(steeringAxis ,0, (Time.deltaTime * 10f * centeringSpeed));
        }
        if (Mathf.Abs(wheels[FWL].steerAngle) < 1f)
        {
            steeringAxis = 0f;
        }
        var steeringAngle = steeringAxis * maxSteeringAngle;
        wheels[FWL].steerAngle = Mathf.Lerp(wheels[FWL].steerAngle, steeringAngle, steeringSpeed);
        wheels[FWR].steerAngle = Mathf.Lerp(wheels[FWR].steerAngle, steeringAngle, steeringSpeed);
    }



    void Gas()
    {
        if(VerticalInput == 0) {
            if (!deceleratingCar && !FootOnBreak && Mathf.Abs(carSpeed) > 0)
            {
                //Debug.Log("[Gas]Should Decel");
                //InvokeRepeating("DecelerateCar", 0f, 0.1f);
                deceleratingCar = true;
                
            }
            return;
        }
        //CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car is losing traction, then the car will start emitting particle systems.
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
            PlayDriftingParticles();
        }
        else
        {
            isDrifting = false;
            PlayDriftingParticles();
        }
        // The following part sets the throttle power to 1 smoothly.
        motorAxis = motorAxis + VerticalInput * (Time.deltaTime * 3f);
        motorAxis = Mathf.Clamp(motorAxis, -1, 1);
        //If the car is going backwards, then apply brakes in order to avoid strange
        //behaviours. If the local velocity in the 'z' axis is less than -1f, then it
        //is safe to apply positive torque to go forward.
        if ((motorAxis > 0 && localVelocityZ < -1f) || motorAxis < 0 && localVelocityZ > 1)
        {
            ApplyBreaks();
        }
        else
        {
            if (motorAxis > 0 && Mathf.RoundToInt(carSpeed) < maxSpeed)
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.brakeTorque = 0;
                    wheel.motorTorque = (accelerationMultiplier * 50f) * motorAxis;
                }
            }
            else if (motorAxis < 0 && Mathf.RoundToInt(carSpeed) < maxReverseSpeed)
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.brakeTorque = 0;
                    wheel.motorTorque = (reverseAccelerationMultiplier * 50f) * motorAxis;
                }
            }
            else
            {
                // If the maxSpeed has been reached, then stop applying torque to the wheels.
                // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
                // could be a bit higher than expected.
                ThrottleOff();
            }
        }


    }

    public void ThrottleOff()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = 0;
        }
    }
    // The following method decelerates the speed of the car according to the decelerationMultiplier variable, where
    // 1 is the slowest and 10 is the fastest deceleration. This method is called by the function InvokeRepeating,
    // usually every 0.1f when the user is not pressing W (throttle), S (reverse) or Space bar (handbrake).
    public void DecelerateCar()
    {
        //Debug.Log("Decel");
        if (!deceleratingCar)
        {
            //CancelInvoke("DeceleratingCar");
            return;
        }
        //Debug.Log("[CAR] Decelerating");
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
            PlayDriftingParticles();
        }
        else
        {
            isDrifting = false;
            PlayDriftingParticles();
        }
        // The following part resets the throttle power to 0 smoothly.
        if (motorAxis != 0f)
        {
            if (motorAxis > 0f)
            {
                motorAxis = motorAxis - (Time.deltaTime * 10f);
            }
            else if (motorAxis < 0f)
            {
                motorAxis = motorAxis + (Time.deltaTime * 10f);
            }
            if (Mathf.Abs(motorAxis) < 0.15f)
            {
                motorAxis = 0f;
            }
        }
        Vector3 newVel = carBody.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
        newVel.y = carBody.velocity.y;
        carBody.velocity = newVel;
        // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
        ThrottleOff();
        // If the magnitude of the car's velocity is less than 0.25f (very slow velocity), then stop the car completely and
        // also cancel the invoke of this method.
        xzVel = new Vector2(carBody.velocity.x, carBody.velocity.z);
        //Debug.Log(carBody.velocity);
        if (xzVel.magnitude < 0.5)
        {
            carBody.velocity = new Vector3(0, carBody.velocity.y, 0);
            //Debug.Log("[CAR] cancel");
            //CancelInvoke("DecelerateCar");
            deceleratingCar = false;
        }
    }

    // This function is used to emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
    // depending on the value of the bool variables 'isDrifting' and 'isTractionLocked'.
    public void PlayDriftingParticles()
    {
        try
        {
            if (isDrifting)
            {
                RLWParticleSystem.Play();
                RRWParticleSystem.Play();
            }
            else if (!isDrifting)
            {
                RLWParticleSystem.Stop();
                RRWParticleSystem.Stop();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }

        try
        {
            if ((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f)
            {
                RLWTireSkid.emitting = true;
                RRWTireSkid.emitting = true;
            }
            else
            {
                RLWTireSkid.emitting = false;
                RRWTireSkid.emitting = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }

    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    public void Handbrake()
    {
        if (!FootOnBreak) return;
        if(timeSpentBreaking > maxDriftTime)
        {
            deceleratingCar = true;
            DecelerateCar();
            recoveringTraction = true;
            RecoverTraction();
            return;
        }
        //CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        //CancelInvoke("RecoverTraction");
        recoveringTraction = false;
        // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
        // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
        // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
        driftingAxis = driftingAxis + (Time.deltaTime);
        float secureStartingPoint = driftingAxis * (frictionInfo[0].externumSlip + flatDrift) * handbrakeDriftMultiplier;

        if (secureStartingPoint < frictionInfo[0].externumSlip)
        {
            driftingAxis = (frictionInfo[0].externumSlip + flatDrift) / (frictionInfo[0].externumSlip + flatDrift) * handbrakeDriftMultiplier;
        }
        driftingAxis = Mathf.Min(driftingAxis, 1);
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car lost its traction, then the car will start emitting particle systems.

        isDrifting = (Mathf.Abs(localVelocityX) > 2.5f);

        //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
        //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
        // = 1f.
        if (driftingAxis < 1f)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                frictionInfo[i].curve.extremumSlip = (frictionInfo[0].externumSlip + flatDrift) * handbrakeDriftMultiplier * driftingAxis;
                wheels[i].sidewaysFriction = frictionInfo[i].curve;
            }
        }

        // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
        // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
        isTractionLocked = true;
        PlayDriftingParticles();

    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    public void RecoverTraction()
    {
        isTractionLocked = false;
        driftingAxis = driftingAxis - (Time.deltaTime / 1.5f);
        if (driftingAxis < 0f)
        {
            driftingAxis = 0f;
        }

        //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
        //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
        // car's grip.
        if (frictionInfo[0].curve.extremumSlip > frictionInfo[0].externumSlip)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                frictionInfo[i].curve.extremumSlip = (frictionInfo[0].externumSlip + flatDrift) * handbrakeDriftMultiplier * driftingAxis;
                wheels[i].sidewaysFriction = frictionInfo[i].curve;
            }

            recoveringTraction = true;

        }
        else if (frictionInfo[0].curve.extremumSlip < (frictionInfo[0].externumSlip + flatDrift))
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                frictionInfo[i].curve.extremumSlip = frictionInfo[i].externumSlip;
                wheels[i].sidewaysFriction = frictionInfo[i].curve;
            }

            driftingAxis = 0f;
            recoveringTraction = false;
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

    void ApplyBreaks()
    {
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = brakeForce;
        }
    }
}
