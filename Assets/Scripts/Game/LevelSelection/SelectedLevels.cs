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
        [field: SerializeField] public int selectedIndex { get; private set; }

        public void Init(QuestLineList questLines)
        {
            var dungeons = questLines.DungeonFileSos;
            for (var i = 0; i < Levels.Count; ++i)
            {
                Levels[i].Init(questLines, dungeons[0]);
            }
        }

        public LevelData GetCurrentLevel()
        {
            return Levels[selectedIndex];
        }

        public void SelectLevel(LevelData selectedLevel)
        {
            if (selectedLevel == null)
            {
                selectedIndex = 0;
                return;
            }
            for (var i = 0; i < Levels.Count; ++i)
            {
                if (Levels[i] != selectedLevel) continue;
                selectedIndex = i;
                break;
            }
        }
    }
}