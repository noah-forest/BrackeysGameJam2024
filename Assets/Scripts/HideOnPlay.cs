using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer.enabled = false;
    }
}
