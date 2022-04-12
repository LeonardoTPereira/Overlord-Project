using System;
using System.Collections.Generic;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelSelection
{
    [CreateAssetMenu(fileName = "SelectedLevels", menuName = "Overlord-Project/SelectedLevels", order = 0)]
    [Serializable]
    public class SelectedLevels : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; set; }

        public void Init(QuestLine questLine)
        {
            Levels = new List<LevelData>();
            var dungeons = questLine.DungeonFileSos;
            foreach (var dungeon in dungeons)
            {
                Levels.Add(new LevelData(questLine, dungeon));
            }
        }
    }
}