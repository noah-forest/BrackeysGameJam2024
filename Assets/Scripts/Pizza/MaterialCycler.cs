using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCycler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] bool useStartingmat = true;
    [SerializeField] int startingMat = 1;
    [SerializeField] Material[] materials;
    void Start()
    {
        if(useStartingmat)meshRenderer.material = materials[startingMat];
    }

    public void ChangeMaterial(int idx)
    {
        idx = Math.Clamp(idx, 0, materials.Length);
        meshRenderer.material = materials[idx];
    }
}
