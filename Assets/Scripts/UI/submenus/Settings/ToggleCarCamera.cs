using UnityEngine;
using UnityEngine.UI;

public class ToggleCarCamera : MonoBehaviour
{
	public Toggle modern;
	public Toggle classic;

	private GameManager gameManager;

	private string camMode;

	private void Awake()
	{
		camMode = PlayerPrefs.GetString("carCamera", "modern");

		if (camMode == "classic")
		{
			modern.isOn = false;
			classic.isOn = true;
		}
		else
		{
			classic.isOn = false;
			modern.isOn = true;
		}
	}

	private void Start()
	{
		gameManager = GameManager.singleton;
		modern.onValueChanged.AddListener(SetControls);
	}

	private void SetControls(bool state)
	{
		if (!state)
		{
			PlayerPrefs.SetString("carCamera", "classic");
			gameManager.cameraModeChanged.Invoke("classic");
		}
		else
		{
			PlayerPrefs.SetString("carCamera", "modern");
			gameManager.cameraModeChanged.Invoke("modern");
		}
	}
}

