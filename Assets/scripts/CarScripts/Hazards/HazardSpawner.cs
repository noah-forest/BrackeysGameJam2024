using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : ObsitcalGenerator
{
    // Start is called before the first frame update
    

    void Start()
    {
        for(int i = 0; i < maxObsticales;  i++)
        {
            if (RollChanceToSpawnNothing())
            {
                Debug.Log($"{gameObject.name} spawning nothing");
                continue;
            }
            Spawn();
        }
    }
}
