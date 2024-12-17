using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
	public ResolutionManager resolutionManager;

	public Toggle fullScreenToggle;
	private bool isFullScreen = false;
	private int screenInt;

	private void Awake()
	{
		screenInt = PlayerPrefs.GetInt("toggleState");
		if(screenInt == 1)
		{
			isFullScreen = true;
			fullScreenToggle.isOn = true;
			resolutionManager.DisableResolution();
		} else
		{
			isFullScreen = false;
			resolutionManager.EnableResolution();
			fullScreenToggle.isOn = false;
		}
	}

	public void SetFullscreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
		
		if(isFullScreen == false)
		{
			resolutionManager.EnableResolution();
			PlayerPrefs.SetInt("toggleState", 0);
		} else
		{
			resolutionManager.DisableResolution();
			PlayerPrefs.SetInt("toggleState", 1);
		}
	}
}
