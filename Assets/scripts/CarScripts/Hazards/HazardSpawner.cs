using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : ObsitcalGenerator
{
    // Start is called before the first frame update
    [SerializeField] int minObsticales;
    [SerializeField] int maxObsticales;

    void Start()
    {
        while (obsticalesGenerated < minObsticales && obsticalesGenerated < maxObsticales)
        {
            Spawn();
        }
    }
}
