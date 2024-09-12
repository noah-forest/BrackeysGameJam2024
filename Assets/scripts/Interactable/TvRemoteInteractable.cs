using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvRemoteInteractable : PizzaModeInteractable
{

    enum RemoteType
    {
        toggleAudio,
        togglePower,
    }

    [SerializeField] SelectRandomVideo tv;
    [SerializeField] GameObject videoScreen;
    [SerializeField] AudioSource videoSoundPlayer;
    [SerializeField] RemoteType remoteType;
    [SerializeField] MaterialCycler buttonDisplay;
    static bool tvOn = true;
    // Start is called before the first frame update

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        switch (remoteType)
        {
            case RemoteType.toggleAudio:
                ChangeAudio(gameObject);
                break; 
            case RemoteType.togglePower:
                ChangePower(gameObject);
                break;
        }

    }
    public void ChangeAudio(GameObject obj)
    {
        //Debug.Log("[changeAud]Tv video: " + tvOn);
        
        displayText = "[LMB] Mute";
        obj.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
        
        if (tvOn && !videoSoundPlayer.mute)
        {
            displayText = "[LMB] Toggle Power";
            obj.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
            
            videoSoundPlayer.mute = !videoSoundPlayer.mute;
            int matIdx = videoSoundPlayer.mute ? 2 : 1;
            buttonDisplay.ChangeMaterial(matIdx);
        }
        else
        {
            videoSoundPlayer.mute = false;
            ChangePower(obj);

            if (tvOn)
            {
                displayText = "[LMB] Mute";
                obj.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
            }
            else
            {
                displayText = "[LMB] Toggle Power";
                obj.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
            }
        }

    }
    public void ChangePower(GameObject obj)
    {
        tvOn = tv.TogglePower();
        int matIdx  = tvOn ? 1 : 0;
        Debug.Log("[changePow]Tv video: " + tvOn);
        buttonDisplay.ChangeMaterial(matIdx);
        videoScreen.SetActive(!(bool)videoScreen.activeSelf);
    }
}
