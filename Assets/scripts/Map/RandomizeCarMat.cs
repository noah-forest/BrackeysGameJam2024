using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeCarMat : MonoBehaviour
{
    public List<Material> possibleMats;
    
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        RandomizeMaterial();
    }

    private void RandomizeMaterial()
    {
        var index = Random.Range(0, possibleMats.Count);
        _renderer.material = possibleMats[index];
    }
}
