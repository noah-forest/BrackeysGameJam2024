using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccumProp : PizzaModeInteractable
{
    // Start is called before the first frame update


    enum VacState
    {
        off,
        sucking,
        blowign
    }

    [SerializeField] Wind sucker;
    [SerializeField] Wind blower;
    [SerializeField] AudioSource onSound;
    [SerializeField] MaterialCycler stateLight;
    VacState state;

    void Start()
    {
        state = VacState.off;
        blower.gameObject.SetActive(false);
        sucker.gameObject.SetActive(false);
        onSound.Stop();
    }

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        state += 1;
        if(((int)state) > 2) { state = VacState.off; }
        Debug.Log("Vaccume Sate: " + state.ToString()) ;
        switch(state)
        {
            case VacState.off:
                blower.gameObject.SetActive(false);
                sucker.gameObject.SetActive(false);
                stateLight.ChangeMaterial(0);
                onSound.Stop();
                break;
            case VacState.sucking:
                blower.gameObject.SetActive(false);
                sucker.gameObject.SetActive(true);
                stateLight.ChangeMaterial(1);
                onSound.Play();
                break;
            case VacState.blowign:
                blower.gameObject.SetActive(true);
                sucker.gameObject.SetActive(false);
                stateLight.ChangeMaterial(2);
                onSound.Play();
                break;
        }
    }
}
