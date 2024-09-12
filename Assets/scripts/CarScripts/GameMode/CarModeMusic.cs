using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarModeMusic : MonoBehaviour
{
    CarModeManager modeManager;

    [SerializeField] int _currentTrack = 0;
    public int CurrentTrack
    {
        get { return _currentTrack; }
        set
        {
            if (value >= tracks.Length)
            {
                _currentTrack = 0;
            }else if ( value < 0)
            {
                _currentTrack = tracks.Length - 1;
            }
            else
            {
                _currentTrack = value;
            }
        }
    }
    [SerializeField] AudioClip[] tracks;
    [SerializeField] AudioSource player;
    [SerializeField][Range(0,1)] float minVolume;
    [SerializeField][Range(0, 1)] float maxVolume;
    [SerializeField] float fadeInRate = 0.1f;
    [SerializeField] float fadeOutRate = 0.01f;
    public bool IsPlaying { get; private set; }
    public bool HasPlayed { get; private set; }

    private void Start()
    {
        modeManager = CarModeManager.singleton;
        if (_currentTrack == -1)
        {
            CurrentTrack = Random.Range(0,tracks.Length);
        }
        CurrentTrack = _currentTrack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlaying && !HasPlayed)
        {
            player.clip = (tracks[CurrentTrack]);
            player.volume = minVolume;
            player.Play();
            IsPlaying = true;
        }
    }

    public void Update()
    {
        if (IsPlaying)
        {
            if (modeManager._pizzasToDeliver > 0)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

    }

    void FadeIn()
    {
        player.volume = Mathf.Lerp(player.volume, maxVolume, Time.deltaTime * fadeInRate);
    }
    void FadeOut()
    {
        player.volume = Mathf.Lerp(player.volume, 0, Time.deltaTime * fadeOutRate);
        if (player.volume <= 0)
        {
            player.Stop();
            IsPlaying = false;
            HasPlayed = true;
        }
        //Debug.Log(player.volume);
    }
}
