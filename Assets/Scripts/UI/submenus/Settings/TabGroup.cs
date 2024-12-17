using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
	public List<TabButton> tabs;
	public List<GameObject> pagesToToggle;

	public Color tabIdle;
	public Color tabHover;
	public Color tabActive;

	public TabButton selectedTab;

	public void Subscribe(TabButton button)
	{
		tabs ??= new List<TabButton>();

		tabs.Add(button);
	}

	public void OnTabEnter(TabButton button)
	{
		ResetTabs();
		if (selectedTab == null || button != selectedTab)
		{
			button.background.color = tabHover;
		}
	}

	public void OnTabExit()
	{
		ResetTabs();
	}

	public void OnTabSelected(TabButton button)
	{
		selectedTab = button;
		ResetTabs();
		button.background.color = tabActive;
		int index = button.transform.GetSiblingIndex();
		for (int i = 0; i < pagesToToggle.Count; i++)
		{
			if (i == index)
			{
				pagesToToggle[i].SetActive(true);
			}
			else
			{
				pagesToToggle[i].SetActive(false);
			}
		}
	}

	public void ResetTabs()
	{
		foreach (TabButton button in tabs)
		{
			if (selectedTab != null && button == selectedTab) { continue; }
			button.background.color = tabIdle;
		}
	}
}
