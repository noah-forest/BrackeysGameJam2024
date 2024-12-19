using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscribeToManager : MonoBehaviour
{
	private TextureManager textureManager;
	private Camera cam;

	private void Start()
	{
		textureManager = TextureManager.instance;

		float index = PlayerPrefs.GetFloat("crunchLevel", 0);
		cam.targetTexture = textureManager.renderTextures[(int)index];
	}
}
