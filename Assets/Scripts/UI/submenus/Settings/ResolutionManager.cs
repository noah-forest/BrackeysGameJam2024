using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
	public TextMeshProUGUI resTitleText;
	public TMP_Dropdown dropdown;
	public Image icon;

	private Resolution[] resolutions;

	const string resName = "resolutionOption";

	private void Awake()
	{
		dropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
		{
			PlayerPrefs.SetInt(resName, dropdown.value);
			PlayerPrefs.Save();
		}));
	}

	private void Start()
	{
		resolutions = Screen.resolutions;

		dropdown.ClearOptions();

		List<string> options = new();

		int curIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if(resolutions[i].width == Screen.currentResolution.width && 
				resolutions[i].height == Screen.currentResolution.height)
			{
				curIndex = i;
			}
		}

		dropdown.AddOptions(options);
		dropdown.value = PlayerPrefs.GetInt(resName, curIndex);
		dropdown.RefreshShownValue();
	}

	public void SetResolution(int index)
	{
		Resolution resolution = resolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void DisableResolution()
	{
		resTitleText.color = new Color32(130,130,130,255);
		dropdown.captionText.color = new Color32(130, 130, 130, 255);
		icon.color = new Color32(130, 130, 130, 255);
		dropdown.interactable = false;
	}

	public void EnableResolution()
	{
		resTitleText.color = Color.white;
		dropdown.captionText.color = Color.white;
		icon.color = Color.white;
		dropdown.interactable = true;
	}
}
