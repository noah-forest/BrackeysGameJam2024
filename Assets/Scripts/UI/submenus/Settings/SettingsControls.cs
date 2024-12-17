using UnityEngine;

public class SettingsControls : MonoBehaviour
{
	public GameObject settingsMenu;

	private bool isOpen;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
		{
			CloseSettings();
		}
	}

	public void OpenSettings()
	{
		isOpen = true;
		settingsMenu.SetActive(true);
	}

	public void CloseSettings()
	{
		isOpen = false;
		settingsMenu.SetActive(false);
	}
}