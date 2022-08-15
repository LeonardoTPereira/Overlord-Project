using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelSelection
{
    [CreateAssetMenu(fileName = "SelectedLevels", menuName = "Overlord-Project/SelectedLevels", order = 0)]
    [Serializable]
    public class SelectedLevels : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; set; }
        [SerializeField] private int selectedIndex;

        public void Init(QuestLineList questLines)
        {
            var dungeons = questLines.DungeonFileSos;
            for (var i = 0; i < dungeons.Count; ++i)
            {
                Levels[i].Init(questLines, dungeons[i]);
            }
        }

        public LevelData GetCurrentLevel()
        {
            return Levels[selectedIndex];
        }

        public void SelectLevel(int index)
        {
            selectedIndex = index;
        }
    }
}