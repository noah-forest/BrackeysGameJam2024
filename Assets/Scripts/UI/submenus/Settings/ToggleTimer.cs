using UnityEngine;
using UnityEngine.UI;

public class ToggleTimer : MonoBehaviour
{
	public GameManager Manager;
	public Toggle timerToggle;
	private int timerInt;

	private void Awake()
	{
		timerInt = PlayerPrefs.GetInt("timerState");
		if (timerInt == 1)
		{
			timerToggle.isOn = true;
		}
		else
		{
			timerToggle.isOn = false;
		}
	}

	public void SetTimer(bool state)
	{
		Manager.speedrunTimer.SetActive(state);

		if (!state)
		{
			Manager.speedrunTimerToggled = false;
			PlayerPrefs.SetInt("timerState", 0);
		}
		else
		{
			Manager.speedrunTimerToggled = true;
			PlayerPrefs.SetInt("timerState", 1);
		}
	}
}
