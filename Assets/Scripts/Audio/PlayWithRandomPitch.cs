using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayWithRandomPitch : MonoBehaviour
{
    private AudioSource m_AudioSource;

	private void Start()
	{
		m_AudioSource = GetComponent<AudioSource>();
	}

	public void PlayRandomPitch()
	{
		m_AudioSource.pitch = Random.Range(0.9f, 1.2f);
		m_AudioSource.Play();
	}
}
