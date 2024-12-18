using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulateAchievements : MonoBehaviour
{
	public GameObject achievementEntry;

	private List<AchievementEntry> achievementEntries;

	private void Start()
	{
		CreateEntries();
	}

	private void CreateEntries()
	{
		LoadEntries();
		foreach(var entry in achievementEntries)
		{
			var obj = Instantiate(achievementEntry, transform);

			var entryInfo = obj.GetComponent<A_EntryInfo>();
			entryInfo.icon.sprite = entry.lockedIcon;
			entryInfo.nameText.text = entry.Name;
			entryInfo.descriptionText.text = entry.Description;
		}
	}

	private void LoadEntries()
	{
		achievementEntries = Resources.LoadAll<AchievementEntry>("SOs/Achievements").ToList();
	}
}
