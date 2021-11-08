﻿using System.Collections.Generic;
using System.IO;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    public class QuestLine : ScriptableObject
    {
        public List<QuestSO> graph;
        public QuestDungeonsParameters DungeonParametersForQuestLine { get; }
        public QuestEnemiesParameters EnemyParametersForQuestLine { get; }
        public QuestNpcsParameters NpcParametersForQuestLine { get; }
        public QuestItemsParameters ItemParametersForQuestLine { get; }

        public List<DungeonFileSO> DungeonFileSos { get; }
        public List<EnemySO> EnemySos { get; }
        public List<NpcSO> NpcSos { get; }
        public List<ItemSO> ItemSos { get; }

        public QuestLine()
        {
            graph = new List<QuestSO>();
            DungeonFileSos = new List<DungeonFileSO>();
            EnemySos = new List<EnemySO>();
            NpcSos = new List<NpcSO>();
            ItemSos = new List<ItemSO>();
        }

        public void CreateAsset(PlayerProfile.PlayerProfileCategory playerProfileCategory)
        {
            // Define the JSON file extension
            const string extension = ".asset";

            // Build the target path
            string target = Application.dataPath;
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Experiment";
            target += Constants.SEPARATOR_CHARACTER + playerProfileCategory.ToString();

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            string fileName = "Narrative_" + graph[0] + extension;

            AssetDatabase.CreateAsset(this, fileName);

            /*            Resources.UnloadUnusedAssets();
            #if UNITY_EDITOR
                        AssetDatabase.Refresh();
            #endif*/
        }
    }
}