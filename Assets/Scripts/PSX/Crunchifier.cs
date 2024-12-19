using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Crunchifier : MonoBehaviour
{
	private Slider slider;
	private TextureManager textureManager;

	private void Awake()
	{
		slider = GetComponent<Slider>();

		slider.onValueChanged.AddListener(SetCrunchLevel);
	}

	private void Start()
	{
		textureManager = TextureManager.instance;
		slider.value = PlayerPrefs.GetFloat("crunchLevel", 0);
	}

	private void OnDisable()
	{
		SetCrunchLevel(slider.value);
	}

	private void SetCrunchLevel(float level)
	{
		if(SceneManager.GetActiveScene().name != "main")
			textureManager.UpdateTexture(level);

		PlayerPrefs.SetFloat("crunchLevel", level);
	}
}
