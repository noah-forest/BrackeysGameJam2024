using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
	[SerializeField] AudioMixer mixer;
	[SerializeField] Slider musicSlider;
	[SerializeField] Slider sfxSlider;

	public const string MIXER_MUSIC = "musicVolume";
	public const string MIXER_SFX = "sfxVolume";

	private SetText musicValueText;
	private SetText sfxValueText;

	private void Awake()
	{
		musicSlider.onValueChanged.AddListener(SetMusicVolume);
		sfxSlider.onValueChanged.AddListener(SetSFXVolume);

		musicValueText = musicSlider.GetComponent<SetText>();
		sfxValueText = sfxSlider.GetComponent<SetText>();
	}

	private void Start()
	{
		musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 50f);
		sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 50f);

		SetMusicVolume(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 50f));
		SetSFXVolume(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 50f));

		musicValueText.textComponent.text = musicSlider.value.ToString();
		sfxValueText.textComponent.text = sfxSlider.value.ToString();
	}

	private void OnDisable()
	{
		PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
		PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
	}

	private void SetMusicVolume(float volume)
	{
		if(volume < 1)
		{
			volume = 0.001f;
		}

		mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume / 100) * 20f);

		musicValueText.textComponent.text = musicSlider.value.ToString();
	}

	private void SetSFXVolume(float volume)
	{
		if (volume < 1)
		{
			volume = 0.001f;
		}

		sfxValueText.textComponent.text = sfxSlider.value.ToString();
		mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume / 100) * 20f);
	}
}
