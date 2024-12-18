using UnityEngine;
using UnityEngine.UI;

public class ToggleTimer : MonoBehaviour
{
	private Toggle timerToggle;
	private GameManager gameManager;
	private int timerInt;

	private void Awake()
	{
		timerToggle = GetComponent<Toggle>();
		gameManager = GameManager.singleton;
		timerToggle.onValueChanged.AddListener(SetTimer);

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
		gameManager.speedrunTimer.SetActive(state);

		if (!state)
		{
			gameManager.speedrunTimerToggled = false;
			PlayerPrefs.SetInt("timerState", 0);
		}
		else
		{
			gameManager.speedrunTimerToggled = true;
			PlayerPrefs.SetInt("timerState", 1);
		}
	}
}
