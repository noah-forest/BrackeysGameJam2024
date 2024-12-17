using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SetText : MonoBehaviour
{
	public TextMeshProUGUI textComponent;

	public void SetTextFromSlider(Slider slider)
	{
		textComponent.text = $"{slider.value}";
	}
}
