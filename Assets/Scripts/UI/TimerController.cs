using System;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
	public static TimerController singleton;
	public TextMeshProUGUI timeCounter;

	public GameObject timer;

	public bool timerGoing = false;

	public TimeSpan time;

	private GameManager gameManager;

	private float elaspedTime;

	private void Awake()
	{
		if (singleton)
		{
			Destroy(this.gameObject);
			return;
		}

		singleton = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		gameManager = GameManager.singleton;

		var timerState = PlayerPrefs.GetInt("timerState");
		if (timerState == 1)
		{
			gameManager.speedrunTimerToggled = true;
			ShowTimer();
		}

		elaspedTime = 0;
	}

	private void Update()
	{
		if (timerGoing)
		{
			elaspedTime += Time.deltaTime;
			time = TimeSpan.FromSeconds(elaspedTime);

			if(time.TotalHours > 0)
			{
				timeCounter.text = time.ToString("mm':'ss'.'ff");
			} else
			{
				timeCounter.text = time.ToString("hh':'mm':'ss'.'ff");
			}
		}
	}

	public void StartTimer()
	{
		timerGoing = true;
	}

	public void PauseTimer()
	{
		timerGoing = false;
	}

	public void ResetTimer()
	{
		timerGoing = false;
		elaspedTime = 0;
	}

	public void ShowTimer()
	{
        if(gameManager.speedrunTimerToggled)
			timer.SetActive(true);
	}

	public void HideTimer()
	{
		if (gameManager.speedrunTimerToggled)
			timer.SetActive(false);
	}
}
