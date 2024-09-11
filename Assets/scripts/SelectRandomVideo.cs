using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class SelectRandomVideo : MonoBehaviour
{
    public List<VideoClip> videoClips;
    
    private VideoPlayer videoPlayer;

    private bool active;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        ChangeVideo();
    }

    private void Update()
    {
        if (!videoPlayer.isPlaying && active)
        {
            ChangeVideo();
        }
    }

    private void ChangeVideo()
    {
        videoPlayer.Stop();
        active = false;
        
        var videoIndex = Random.Range(0, videoClips.Count);
        
        Debug.Log(videoClips[videoIndex].name);
        
        videoPlayer.clip = videoClips[videoIndex];
        videoPlayer.Play();
        StartCoroutine(WaitForVideo());
    }

    private IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds(0.3f);
        active = true;
    }
}
