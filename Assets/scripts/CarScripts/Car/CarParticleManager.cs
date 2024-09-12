using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleManager : MonoBehaviour
{

    [SerializeField] GameObject[] turboVisuals;
    [SerializeField] GameObject[] smearVisuals;
    [SerializeField] GameObject[] breakVisuals;

    [SerializeField] CarController car;


    private void Update()
    {
        turboVisuals[0].SetActive(car.FootOnTurbo && car.motorAxis > 0);
        turboVisuals[1].SetActive(car.FootOnTurbo && car.motorAxis < 0);

        //foreach(GameObject b in breakVisuals){
        //    b.SetActive(car.FootOnBreak);
        //}

        //for(int i = 0; i < smearVisuals.Length; i++) 
        //{
        //    smearVisuals[i].SetActive((i <= 1 && car.motorAxis > 0) || (i > 1 && car.motorAxis < 0));
        //}
        
    }
}
