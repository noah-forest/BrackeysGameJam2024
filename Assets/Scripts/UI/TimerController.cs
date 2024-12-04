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
		elaspedTime = 0;
	}

	private void Update()
	{
		if (timerGoing)
		{
			elaspedTime += Time.deltaTime;
			time = TimeSpan.FromSeconds(elaspedTime);
			timeCounter.text = time.ToString("hh':'mm':'ss'.'ff");
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
		timer.SetActive(true);
	}

	public void HideTimer()
	{
		timer.SetActive(false);
	}
}
