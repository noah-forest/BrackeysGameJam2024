using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Achievements/AchievementEntry")]
public class AchievementEntry : ScriptableObject
{
	public Sprite unlockedIcon;
	public Sprite lockedIcon;
	public string Name;

	[TextArea]
	public string Description;

	public bool Unlocked;
}
