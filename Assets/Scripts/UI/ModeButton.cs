using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject hover;
	public TextMeshProUGUI modeDesc;

	[TextArea]
	public string description;

	private Animator hoverAnim;

	private void Start()
	{
		hoverAnim = hover.GetComponent<Animator>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		modeDesc.text = description;
		hoverAnim.SetTrigger("hover");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hoverAnim.SetTrigger("left");
	}
}
