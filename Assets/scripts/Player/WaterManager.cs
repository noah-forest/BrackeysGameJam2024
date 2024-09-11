using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] ParticleSystem emmiter;
    bool joinking = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.E))
        {
            if (!joinking)
            {
                var emmission = emmiter.emission;
                emmission.rateOverTime = 30;

                joinking = true;
            }

        }
        else if (joinking)
        {
            var emmission = emmiter.emission;
            emmission.rateOverTime = 0;
            emmiter.Play();
            joinking = false;
        }
    }
}
