using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompletedOrderUI : MonoBehaviour
{
    public GameObject thumbsUp;
    public TextMeshProUGUI orderName;
    public TextMeshProUGUI orderScore;
	public AudioSource sfxPlayer;
	public AudioClip scoredClip;
	public AudioClip lostClip;
}