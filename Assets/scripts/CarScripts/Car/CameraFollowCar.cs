using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollowCar : MonoBehaviour
{
    enum FollowMode
    {
        useTarget,
        useFixed,
        useFixedWithRotation
    }
    [SerializeField] CarMaster car;
    [SerializeField] Camera cam;
    [SerializeField] FollowMode mode = FollowMode.useTarget;

    [SerializeField] Transform fixedPosition;

    Vector3 initialFixedPos;
    Vector3 initialFixedRotEuler;

    [SerializeField] Transform targetPosition;

    Vector3 initialTargetPos;
    Vector3 initialTargetRotEuler;

    [SerializeField] Transform carXZ;
    [SerializeField] float lookAlpha = 0.1f;
    [SerializeField] float moveAlpha = 0.1f;
    [SerializeField][Range(0, 40)] float minFOV;
    [SerializeField][Range(30, 50)] float maxFoV;
    [SerializeField][Range(0, 2000)] float velocityThreshold = 100;

    [SerializeField]
    [Range(0, 1)]
    float cameraSlowOnHit = 0.1f;
    float hitRecencyAlphaModifier = 1;
    [SerializeField]
    float hitModifierDecayAlpha = 0.1f;

    float floorY;

    Vector3 lastTarget;
    Vector3 lastPosition;

    float distanceToCar;
    float initialDistanceToCar;

    private void Start()
    {
        initialFixedPos = fixedPosition.position;
        initialFixedRotEuler = fixedPosition.rotation.eulerAngles;

        initialTargetPos = targetPosition.position;
        initialTargetRotEuler = targetPosition.rotation.eulerAngles;
        floorY = car.transform.position.y;
        lastTarget = car.transform.position;
        car.health.carDamaged.AddListener(SlowCameraOnHit);
        initialDistanceToCar = Vector3.Distance(car.transform.position, transform.position);
    }

    void SlowCameraOnHit()
    {
        hitRecencyAlphaModifier = cameraSlowOnHit;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Vector3.zero;
        Quaternion targetRot = Quaternion.identity;
        float carDistRatio = Mathf.Clamp01(Vector3.Distance(car.transform.position, transform.position) / initialDistanceToCar);

        switch (mode)
        {
            case FollowMode.useTarget:
                carXZ.transform.position = new Vector3(car.transform.position.x, floorY, car.transform.position.z);
                Vector3 eulerzoz = car.transform.rotation.eulerAngles;
                carXZ.rotation = Quaternion.Euler(0, eulerzoz.y, 0);
                targetPos = targetPosition.position;
                targetPos.y = initialTargetPos.y;
                //Vector3 eulRot = initialTargetRotEuler;

                //eulRot.y = targetPosition.rotation.y;

                //targetRot = Quaternion.Euler(eulRot);
                hitRecencyAlphaModifier = Mathf.Lerp(hitRecencyAlphaModifier, 1, hitModifierDecayAlpha);
                Vector3 lookTarget = Vector3.Lerp(lastTarget, carXZ.position, lookAlpha * hitRecencyAlphaModifier * carDistRatio);
                transform.LookAt(lookTarget);
                lastTarget = lookTarget; 
                break;
            case FollowMode.useFixed:
                targetPos = car.transform.position;
                targetPos.y = initialFixedPos.y;
                targetRot = Quaternion.Euler(initialFixedRotEuler);
                break;
        }






        //if (mimicRotation)
        //{
        //    transform.rotation = car.transform.rotation;
        //    transform.Rotate(Vector3.right, 90);
        //}
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFoV, car.body.velocity.magnitude / velocityThreshold);
        transform.position = Vector3.Lerp(transform.position,targetPos, moveAlpha * hitRecencyAlphaModifier * carDistRatio);
       // transform.rotation = targetRot;
    }
}
