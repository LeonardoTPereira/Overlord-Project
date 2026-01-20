using Overlord.ProfileAnalyst;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using static Util.Enums;
using System.Linq;

public class YeeProfileCalculator : IPlayerProfileCalculator
{
    public static Dictionary<string, Func<int, float>> StartSymbolWeights { get; protected set; }
    protected static Dictionary<string, float> _questWeightsByType;

    public IPlayerProfile CreateProfileFromPlayerProfileSO(PlayerProfileSO playerProfileSO)
    {
        if (playerProfileSO is YeePlayerProfileSO yeeProfileSO)
        {
            SetProfileWeightsFromPlayerProfileSO(yeeProfileSO);
            return CreateProfileWithWeights();
        }
        Debug.Log("NOT RECEIVING a Yee type of PlayerProfileSO, but calling YeeProfileCalculator");
        return null;
    }

    public IPlayerProfile GetRandomPlayerProfile()
    {
        System.Random rand = new System.Random();
        YeePlayerProfileSO randomProfileSO = new YeePlayerProfileSO
        {
            Achievement = rand.Next(1, 101),
            Mastery = rand.Next(1, 101),
            Creativity = rand.Next(1, 101),
            Immersion = rand.Next(1, 101)
        };

        return CreateProfileFromPlayerProfileSO(randomProfileSO);
    }

    private void SetProfileWeightsFromPlayerProfileSO(YeePlayerProfileSO playerProfileSO)
    {
        _questWeightsByType = new Dictionary<string, float>
        {
            {YeePlayerProfile.PlayerProfileCategory.Immersion.ToString(), playerProfileSO.Immersion},
            {YeePlayerProfile.PlayerProfileCategory.Achievement.ToString(), playerProfileSO.Achievement},
            {YeePlayerProfile.PlayerProfileCategory.Mastery.ToString(), playerProfileSO.Mastery},
            {YeePlayerProfile.PlayerProfileCategory.Creativity.ToString(), playerProfileSO.Creativity}
        };
    }

    protected static YeePlayerProfile CreateProfileWithWeights()
    {
        var playerProfile = new YeePlayerProfile
        {
            AchievementPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Achievement.ToString()],
            MasteryPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Mastery.ToString()],
            CreativityPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Creativity.ToString()],
            ImmersionPreference = _questWeightsByType[YeePlayerProfile.PlayerProfileCategory.Immersion.ToString()]
        };

        CalculateStartSymbolWeights(playerProfile);
        var favoriteQuest = _questWeightsByType.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        playerProfile.SetProfileFromFavoriteQuest(favoriteQuest);
        playerProfile.Normalize();
        return playerProfile;
    }

    protected static void CalculateStartSymbolWeights(YeePlayerProfile playerProfile)
    {
        float creativityPreference = RemoveZeros(playerProfile.CreativityPreference);
        float achievementPreference = RemoveZeros(playerProfile.AchievementPreference);
        float masteryPreference = RemoveZeros(playerProfile.MasteryPreference);
        float immersionPreference = RemoveZeros(playerProfile.ImmersionPreference);

        float normalizeConst = creativityPreference + achievementPreference;
        normalizeConst += masteryPreference + immersionPreference;

        float talkWeight = RemoveZeros((100 * immersionPreference / normalizeConst));
        float getWeight = RemoveZeros((100 * achievementPreference / normalizeConst));
        float killWeight = RemoveZeros((100 * masteryPreference / normalizeConst));
        float exploreWeight = RemoveZeros((100 * creativityPreference / normalizeConst));

        StartSymbolWeights = new Dictionary<string, Func<int, float>>
            {
                {Constants.ImmersionQuest, _ => talkWeight},
                {Constants.AchievementQuest, _ => getWeight},
                {Constants.MasteryQuest, _ => killWeight},
                {Constants.CreativityQuest, _ => exploreWeight}
            };
    }

    protected static float RemoveZeros(float playerPreference)
    {
        if (playerPreference > 1)
        {
            return playerPreference;
        }
        return (float)QuestWeights.Hated;
    }
}
