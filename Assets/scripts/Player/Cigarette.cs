using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : MonoBehaviour
{
    public Mesh[] cigStages;

    public bool smokeable;
    
    public GameObject leftArm;
    
    private MeshFilter filter;
    private int currentCigIndex;
    private int cigCount;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
        InitializeCig();
    }

    public void Smoke()
    {
        ++currentCigIndex;

        var meshInterp = (int)Mathf.Lerp(1, cigStages.Length, (float)currentCigIndex / cigCount);
        
        if (currentCigIndex >= cigCount)
        {
            meshInterp = 0;
            smokeable = false;
            leftArm.SetActive(false);
        }
        
        filter.mesh = cigStages[meshInterp];
    }

    public void InitializeCig()
    {
        cigCount = cigStages.Length;
        smokeable = true;
        currentCigIndex = 0;
        filter.mesh = cigStages[0];
    }
}
