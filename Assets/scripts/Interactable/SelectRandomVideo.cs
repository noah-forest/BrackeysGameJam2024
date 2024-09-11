using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class SelectRandomVideo : MonoBehaviour
{
    public VideoClip[] videoClips;
    List<VideoClip> AllowedClips = new List<VideoClip>();
    
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

        if (AllowedClips.Count <= 0) AllowedClips.AddRange(videoClips);
        var videoIndex = Random.Range(0, AllowedClips.Count);
        
        
        //Debug.Log(AllowedClips[videoIndex].name);
        
        videoPlayer.clip = AllowedClips[videoIndex];
        videoPlayer.Play();
        AllowedClips.RemoveAt(videoIndex);



        StartCoroutine(WaitForVideo());
    }

    private IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds(0.3f);
        active = true;
    }

    public bool TogglePower()
    {
        if(videoPlayer.isPlaying) 
        {
            videoPlayer.Stop();
            active = false;
            gameObject.SetActive(false);
            return false;
        }
        else
        {
            gameObject.SetActive(true);
            ChangeVideo();
            return true;
        }
    }
}
