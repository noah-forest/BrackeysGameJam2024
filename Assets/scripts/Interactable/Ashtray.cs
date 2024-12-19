using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashtray : MonoBehaviour, IInteractable
{

    public PlayerMaster player;
    public ParticleSystem useParticles;
    public string displayString;
    public GameObject buttPrefab;
    public Transform spawnPoint;
    public float buttDuration = 5;

    public bool CanInteract(GameObject interactor)
    {
        return player.smoker.active;
    }

    public string GetDisplayString()
    {
        return displayString;
    }

    public void Interact(GameObject interactor)
    {
        player.smoker.PutDown();
        useParticles.Play();
        Destroy(Instantiate(buttPrefab, spawnPoint.position + Vector3.one  * Random.Range(-0.05f,0.05f), spawnPoint.rotation * Random.rotationUniform,transform), buttDuration);
        
    }

    public void OnLook()
    {
        throw new System.NotImplementedException();
    }

    public void OnLookAway()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PizzaModeManager.singleton.playerMaster;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
