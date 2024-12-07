using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEvent : MonoBehaviour
{
	private AudioSource m_Source;

	private void Start()
	{
		m_Source = GetComponent<AudioSource>();
	}

	public void PlaySoundEffect()
	{
		m_Source.Play();
	}
}
