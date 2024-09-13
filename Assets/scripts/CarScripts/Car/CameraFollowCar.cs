using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCar : MonoBehaviour
{

    [SerializeField] CarMaster car;
    [SerializeField] bool mimicRotation = false;
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = car.transform.position;
        targetPos.y = transform.position.y;
        transform.position = targetPos;
        if (mimicRotation)
        {
            transform.rotation = car.transform.rotation;
            transform.Rotate(Vector3.right, 90);
        }
    }
}
