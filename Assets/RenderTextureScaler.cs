using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureScaler : MonoBehaviour
{
    // const float baseHeight = 10;
    const float desiredAspectRatio = 16f / 9f;
    void Update()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        
        float width = 10*aspectRatio;
        transform.localScale = new Vector3(width, 10, 1);

        if (aspectRatio > desiredAspectRatio)
        {
            transform.localScale = new Vector3(width, (9f/16f) * width, 1);
        }
        else
        {
            transform.localScale = new Vector3(10*desiredAspectRatio, 10, 1);
        }
    }
}
