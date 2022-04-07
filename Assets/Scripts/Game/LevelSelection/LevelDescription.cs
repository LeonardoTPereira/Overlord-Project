﻿using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.LevelSelection
{
    public class LevelDescription : MonoBehaviour
    {
        public string DungeonDescription { get; set; }
        public string QuestDescription { get; set; }

        [field:SerializeField] public TextMeshProUGUI DisplayedText { get; set; }

        private bool _isShowingDungeon;

        public void CreateDescriptions(DungeonFileSo dungeon, QuestLine quests)
        {
            CreateDungeonDescription(dungeon);
            CreateQuestDescription(quests);
            _isShowingDungeon = false;
            ChangeDescription();
        }

        private void CreateDungeonDescription(DungeonFileSo dungeon)
        {
            DungeonDescription = "";
            DungeonDescription += "Rooms: " + dungeon.Rooms.Count;
            DungeonDescription += "\nFitness: " + dungeon.FitnessFromEa;
        }
        
        private void CreateQuestDescription(QuestLine quests)
        {
            QuestDescription = "";
            QuestDescription += "Quests: " + quests.graph.Count;
            QuestDescription += "\nQuest 1 - " + quests.graph[0].QuestName;
        }
        
        public void ChangeDescription(InputAction.CallbackContext context)
        {
            ChangeDescription();
        }

        private void ChangeDescription()
        {
            _isShowingDungeon = !_isShowingDungeon;
            Debug.Log("Change Description!");
            DisplayedText.text = _isShowingDungeon ? DungeonDescription : QuestDescription;
        }
    }
}