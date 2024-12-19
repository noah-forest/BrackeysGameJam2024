using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class AcheivementManager 
{
    public static void StoreStats(string statName, float count)
    {
        
    }
    public static void UnlockAchievement(string achievementName, bool requirementsMet = true)
    {
        if (!requirementsMet && SteamManager.Initialized)
        {

            Steamworks.SteamUserStats.GetAchievement(achievementName, out bool alreadyUnlocked);
            if (!alreadyUnlocked)
            {
                SteamUserStats.SetAchievement(achievementName);
                SteamUserStats.StoreStats();
            }
        }
    }
}
