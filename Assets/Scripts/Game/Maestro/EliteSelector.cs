using System.Collections.Generic;
using Game.LevelSelection;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.Maestro
{
    public static class EliteSelector
    {
        public static void SelectEliteForEachLevel(QuestLine questLine, SelectedLevels selectedLevels)
        {
            selectedLevels.Levels.Clear();
            Debug.Log("Select");
            foreach (var dungeon in questLine.DungeonFileSos)
            {
                selectedLevels.Levels.Add(new LevelData(questLine, dungeon));
            }
        }
    }
}