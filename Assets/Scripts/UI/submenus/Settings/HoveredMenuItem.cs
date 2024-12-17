using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoveredMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public TextMeshProUGUI hoverDesc;

	[TextArea]
	public string description;

	public void OnPointerEnter(PointerEventData eventData)
	{
		hoverDesc.text = description;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hoverDesc.text = "";
	}
}
