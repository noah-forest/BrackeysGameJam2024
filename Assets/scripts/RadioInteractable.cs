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

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        ChangeChannel();

    }

    private void Update()
    {
        if (!player.isPlaying)
        {
            ChangeChannel();
        }
    }

    void ChangeChannel()
    {
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
}
