using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public const string MUSIC_KEY = "MusicVolume";
	public const string SFX_KEY = "SFXVolume";

	[SerializeField] AudioMixer audioMixer;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		LoadVolume();
	}

	private void LoadVolume()
	{
		float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 50f);
		float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 50f);

		audioMixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume / 100) * 20f);
		audioMixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume / 100) * 20f);
	}
}
