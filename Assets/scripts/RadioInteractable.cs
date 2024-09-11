using Grabbing;
using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioInteractable : TriggerInteractor
{
    [SerializeField] AudioSource player;
    [SerializeField] List<AudioClip> trackList;
    int currentTrack = 0;
    bool powerOff;

    public override void Interact(GameObject gameObject)
    {
        if (powerOff) return;
        base.Interact(gameObject);
        ChangeChannel();

    }

    private void Update()
    {
        if (powerOff) return;
        if (!player.isPlaying)
        {
            ChangeChannel();
        }
    }

    void ChangeChannel()
    {
        if (powerOff) return;
        player.Stop();
        if (currentTrack == trackList.Count - 1)
        {
            currentTrack = 0;
        }
        else
        {
            ++currentTrack;
        }
        player.clip = trackList[currentTrack];
        player.Play();
    }

    public void TogglePower()
    {
        if (!powerOff)
        {
            powerOff = true;
            player.Pause();
        }
        else
        {
            powerOff = false;
            player.UnPause();
        }
    }
}
