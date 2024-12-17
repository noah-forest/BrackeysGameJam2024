using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public bool firstSelected;
	public TabGroup tabGroup;
	public Image background;

	private void Start()
	{
		tabGroup.Subscribe(this);
		if (firstSelected)
		{
			tabGroup.OnTabSelected(this);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		tabGroup.OnTabSelected(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tabGroup.OnTabEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tabGroup.OnTabExit();
	}
}
