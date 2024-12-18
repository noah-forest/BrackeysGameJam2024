using UnityEngine;

public class SubmenuControls : MonoBehaviour
{
	public GameObject menu;

	private bool isOpen;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
		{
			CloseMenu();
		}
	}

	public void OpenMenu()
	{
		isOpen = true;
		menu.SetActive(true);
	}

	public void CloseMenu()
	{
		isOpen = false;
		menu.SetActive(false);
	}
}