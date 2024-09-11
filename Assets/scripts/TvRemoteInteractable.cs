using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvRemoteInteractable : RangeInteractable
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
    // Start is called before the first frame update

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        switch (remoteType)
        {
            case RemoteType.toggleAudio:
                ChangeAudio();
                break; 
            case RemoteType.togglePower:
                ChangePower();
                break;
        }

    }
    public void ChangeAudio()
    {
        videoSoundPlayer.mute = !videoSoundPlayer.mute;
        int matIdx = videoSoundPlayer.mute ? 2 : 1;
        buttonDisplay.ChangeMaterial(matIdx);
    }
    public void ChangePower()
    {
        int matIdx  = (bool)tv?.TogglePower() ? 1 : 0;
        buttonDisplay.ChangeMaterial(matIdx);
        videoScreen?.SetActive(!(bool)videoScreen?.activeSelf);
    }
}
