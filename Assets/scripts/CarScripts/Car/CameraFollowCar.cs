using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollowCar : MonoBehaviour
{
    public enum FollowMode
    {
        useDymamic,
        useFixed,
        useFixedWithRotation
    }


    [Serializable] struct CamParams
    {
        public FollowMode followMode;
        public CameraModeParameters settings;

    }

    [SerializeField] FollowMode followMode = FollowMode.useDymamic;
    public FollowMode CameraMode
    {
        get { return followMode; }
        set
        {
            followMode = value;
            InitalizeCameraMode();
        }
    }


    [Header(" ---- Predefined References ---- ")]
    [Space(5)]

    [SerializeField] CamParams[] camParams = new CamParams[3];
    Dictionary<FollowMode, CameraModeParameters> _camParams = new Dictionary<FollowMode, CameraModeParameters>();

    [SerializeField] CarMaster car;
    [SerializeField] Camera cam;

    [SerializeField] Transform fixedPosition;

    Vector3 initialFixedPos;
    Vector3 initialFixedRotEuler;

    [SerializeField] Transform dynamicTarget;

    Vector3 initialTargetPos;
    Vector3 initialTargetRotEuler;

    [SerializeField] Transform carXZ;

    [Space(20)]
    [Header(" ---- Dynamic Camera Settings ----")]
    [Space(5)]

    [Tooltip("How quickly the Camera's Look Target matches the Car's position. The look target is an empty transform that follows the car position")]
    [SerializeField][Range(0, 1)] float carXZMoveAlpha = 1;
    [Tooltip("How quickly should the Camera's position follows the Car (technically follows the lookTarget).")]
    [SerializeField][Range(0, 1)] float moveAlpha = 0.1f;
    [Tooltip("How quickly the Camera rotates to look at the target position")]
    [SerializeField][Range(0, 1)] float lookAlpha = 0.1f;
    [Tooltip("How far in front of the Look target should the camera look")]
    [SerializeField][Range(0, 50)] float lookAheadOffset = 5;
    [Tooltip("How quickly should the Camera rotate to match the lookAheadOffset. This one is weird as it applies a second rotational movement to the camera. If this is high, your normal lookAlpha should probably be lower.")]
    [SerializeField][Range(0, 1)] float lookAheadAlpha = 0.1f;

    [Header("Camera Movement EaseIn")]

    [Tooltip("This is used to scale the movement/rotation of the camera based on how far away it is from its target position. This is jank.")]
    [SerializeField][Range(0, 5)] float distanceToTargetEffectOnSpeed = 5f;
    [Tooltip("The distance at which the largest effect on speed is applied.")]
    [SerializeField][Range(0, 100)] float maxEffectDistance = 10f;

    [Header("Speed Effect on FOV")]

    [Tooltip("FOV when Car is not moving")]
    [SerializeField][Range(0, 40)] float minFOV;
    [Tooltip("FOV when car is at or above it's velocityThreshold")]
    [SerializeField][Range(30, 50)] float maxFoV;
    [Tooltip("The Camera's FOV Scales from the minFOV to the maxFOV based on this parameter.")]
    [SerializeField][Range(0, 2000)] float velocityThreshold = 100;

    [Header("Damage Effect on Camera")]

    [Tooltip("When the car Takes damage, the Camera's Movement/Rotation is slowed down to prevent extreme jittering from the car rapidly moving. This is the The percent of the Movement/Rotation, So 1 = 100% AKA no effect.")]
    [SerializeField][Range(0, 1)] float cameraSlowOnHit = 0.1f;
    [Tooltip("How Quickly the Camera's Movement/Rotation returns to normal after the car takes damage.")]
    [SerializeField][Range(0, 1)]float hitModifierDecayAlpha = 0.1f;
    float hitRecencyAlphaModifier = 1;


    float floorY;

    Vector3 lastTarget;
    Vector3 lastPosition;

    float distanceToCar;
    float initialDistanceToCar;

    private void Start()
    {
        for(int i = 0; i < camParams.Length; i++)
        {
            _camParams.Add(camParams[i].followMode, camParams[i].settings);
        }

        // Camera mode should be able to be changed mid game, so some setup has to be done for both camera modes regardless of mode.


        //Fixed Camera Setup
        initialFixedPos = fixedPosition.position;
        initialFixedRotEuler = fixedPosition.rotation.eulerAngles;

        //Dynamic Camera Setup
        initialTargetPos = dynamicTarget.position;
        initialTargetRotEuler = dynamicTarget.rotation.eulerAngles;
        floorY = car.transform.position.y;
        initialDistanceToCar = Vector3.Distance(car.transform.position, transform.position);
        InitalizeCameraMode();


        

    }

    void InitalizeCameraMode()
    {
        switch (followMode)
        {
            case FollowMode.useFixed:
                car.health.carDamaged.RemoveListener(SlowCameraOnHit);
                break;
            case FollowMode.useDymamic:

                lastTarget = car.transform.position;
                car.health.carDamaged.AddListener(SlowCameraOnHit);

                break;

        }
        LoadCameraSettings();
    }

    void LoadCameraSettings()
    {
        if (!_camParams.ContainsKey(followMode)) return;
        cam.fieldOfView = _camParams[followMode].fov;
        cam.nearClipPlane = _camParams[followMode].nearPlane;
        cam.farClipPlane = _camParams[followMode].farPlane;
    }

    void SlowCameraOnHit()
    {
        hitRecencyAlphaModifier = cameraSlowOnHit;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Vector3.zero;


        switch (followMode)
        {
            case FollowMode.useDymamic:
                Quaternion targetRot = Quaternion.identity;
                float carDistRatio = Mathf.Clamp01(Vector3.Distance(car.transform.position, transform.position) / initialDistanceToCar);
                float targetDistanceRatio = 1;
                carXZ.transform.position = Vector3.Lerp(carXZ.transform.position, new Vector3(car.transform.position.x, floorY, car.transform.position.z) + car.transform.forward * 10, carXZMoveAlpha);

                Vector3 eulerzoz = car.transform.rotation.eulerAngles;
                carXZ.rotation = Quaternion.Euler(0, eulerzoz.y, 0);
                targetPos = dynamicTarget.position;
                targetPos = Vector3.Lerp(targetPos, targetPos + (car.transform.forward * lookAheadOffset), lookAheadAlpha);
                targetPos.y = initialTargetPos.y;
                //Vector3 eulRot = initialTargetRotEuler;
                targetDistanceRatio = Mathf.Clamp01(Mathf.Pow(Vector3.Distance(transform.position, targetPos) / maxEffectDistance, distanceToTargetEffectOnSpeed));
                Debug.Log($"Dist: {Vector3.Distance(transform.position, targetPos)} Ratio: {targetDistanceRatio}");
                //Debug.Log($"Dist: {Vector3.Distance(transform.position, targetPos)} || Ratio: {targetDistanceRatio}");
                //eulRot.y = targetPosition.rotation.y;

                //targetRot = Quaternion.Euler(eulRot);



                hitRecencyAlphaModifier = Mathf.Lerp(hitRecencyAlphaModifier, 1, hitModifierDecayAlpha);
                Vector3 lookTarget = Vector3.Lerp(lastTarget, carXZ.position, lookAlpha * hitRecencyAlphaModifier * carDistRatio);
                lookTarget = Vector3.Lerp(lookTarget, carXZ.position + (car.transform.forward * lookAheadOffset), lookAheadAlpha);

                transform.LookAt(lookTarget);
                lastTarget = lookTarget;


                cam.fieldOfView = Mathf.Lerp(minFOV, maxFoV, car.body.velocity.magnitude / velocityThreshold);
                transform.position = Vector3.Lerp(transform.position, targetPos, moveAlpha * hitRecencyAlphaModifier * carDistRatio * targetDistanceRatio);

                break;
            case FollowMode.useFixed:
                targetPos = car.transform.position;
                targetPos.y = initialFixedPos.y;
                transform.rotation = Quaternion.Euler(initialFixedRotEuler);
                transform.position = targetPos;
                break;
        }






        //if (mimicRotation)
        //{
        //    transform.rotation = car.transform.rotation;
        //    transform.Rotate(Vector3.right, 90);
        //}
        

       // transform.rotation = targetRot;
    }
}
