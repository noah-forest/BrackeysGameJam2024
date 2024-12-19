using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
	public static TextureManager instance;

	public List<RenderTexture> renderTextures = new();
	public List<Material> renderMats = new();

	public MeshRenderer meshRenderer;

	private void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		UpdateTexture(PlayerPrefs.GetFloat("crunchLevel", 0));
	}

	public void UpdateTexture(float index)
	{
		for (int i = 0; i < renderTextures.Count; i++)
		{
			if (i == index)
			{
				meshRenderer.material = renderMats[i];
				Camera.main.targetTexture = renderTextures[i];
			}
		}
	}
}