using UnityEngine;

public class LoadCarCameraSettings : MonoBehaviour
{
	private GameManager gameManager;
	public CameraFollowCar carCam;

	private string camMode;

	private void Awake()
	{
		camMode = PlayerPrefs.GetString("carCamera", "modern");
		SetCamera(camMode);
	}

	private void Start()
	{
		gameManager = GameManager.singleton;
		gameManager.cameraModeChanged.AddListener(SetCamera);
	}

	private void SetCamera(string mode)
	{
		if (mode == "classic")
		{
			carCam.CameraMode = CameraFollowCar.FollowMode.useFixed;
		}
		else
		{
			carCam.CameraMode = CameraFollowCar.FollowMode.useDymamic;
		}
	}
}
