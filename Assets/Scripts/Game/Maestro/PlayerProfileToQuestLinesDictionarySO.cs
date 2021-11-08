﻿using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.Maestro
{
    [CreateAssetMenu(menuName = "DungeonFiles/DungeonFilesDictionarySO")]
    public class PlayerProfileToQuestLinesDictionarySO : ScriptableObject
    {
        public PlayerProfileToQuestLinesDictionary QuestLinesForProfile { get; } =
            new PlayerProfileToQuestLinesDictionary();

        public void Add(string profile,  QuestLine questLine)
        {
            if (!QuestLinesForProfile.ContainsKey(profile))
            {
                QuestLinesForProfile.Add(profile, CreateInstance<QuestLineList>());
            }
            QuestLinesForProfile[profile].QuestLines.Add(questLine);
        }

        public void Remove(string profile)
        {
            if (QuestLinesForProfile.ContainsKey(profile))
                QuestLinesForProfile.Remove(profile);
        }
    }
}