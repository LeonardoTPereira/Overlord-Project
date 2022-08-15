using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.Maestro
{
    [CreateAssetMenu(fileName = "QuestLinesByPlayerProfile", menuName = "Overlord-Project/QuestLinesByPlayerProfile", order = 0)]    
    public class PlayerProfileToQuestLinesDictionarySo : ScriptableObject
    {
        [SerializeField] private PlayerProfileToQuestLinesDictionary questLinesForProfile =
            new PlayerProfileToQuestLinesDictionary();

        
        public PlayerProfileToQuestLinesDictionary QuestLinesForProfile => questLinesForProfile;

        public void Add(string profile,  QuestLineList questLine)
        {
            if (!QuestLinesForProfile.ContainsKey(profile))
            {
                QuestLinesForProfile.Add(profile, new List<QuestLineList>());
            }
            QuestLinesForProfile[profile].Add(questLine);
        }

        public void Remove(string profile)
        {
            if (QuestLinesForProfile.ContainsKey(profile))
                QuestLinesForProfile.Remove(profile);
        }
    }
}