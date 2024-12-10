using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Car/CameraSettings")]

public class CameraModeParameters : ScriptableObject
{
    public float fov = 60f;
    public float nearPlane = 0.3f;
    public float farPlane = 100;


}
