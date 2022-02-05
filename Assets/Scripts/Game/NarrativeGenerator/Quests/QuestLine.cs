using System;
using System.Collections.Generic;
using System.IO;
using Game.GameManager;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Overlord-Project/QuestLine", order = 0)]
    [Serializable]
    public class QuestLine : ScriptableObject
    {
        [SerializeField] public List<QuestSO> graph;
        [SerializeField] private List<DungeonFileSo> _dungeonFileSos;
        [SerializeField] private QuestNpcsParameters _npcParametersForQuestLine;
        [SerializeField] private QuestItemsParameters _itemParametersForQuestLine;
        [SerializeField] private List<EnemySO> _enemySos;
        [SerializeField] private List<NpcSO> _npcSos;
        [SerializeField] private List<ItemSo> _itemSos;
        [SerializeField] private QuestDungeonsParameters dungeonParametersForQuestLine;
        [SerializeField] private QuestEnemiesParameters enemyParametersForQuestLine;
        
        public QuestDungeonsParameters DungeonParametersForQuestLine
        {
            get => dungeonParametersForQuestLine;
            set => dungeonParametersForQuestLine = value;
        }
        public QuestEnemiesParameters EnemyParametersForQuestLine
        {
            get => enemyParametersForQuestLine;
            set => enemyParametersForQuestLine = value;
        }

        public QuestNpcsParameters NpcParametersForQuestLine
        {
            get => _npcParametersForQuestLine;
            set => _npcParametersForQuestLine = value;
        }

        public QuestItemsParameters ItemParametersForQuestLine
        {
            get => _itemParametersForQuestLine;
            set => _itemParametersForQuestLine = value;
        }

        public List<DungeonFileSo> DungeonFileSos
        {
            get => _dungeonFileSos;
            set => _dungeonFileSos = value;
        }

        public List<EnemySO> EnemySos
        {
            get => _enemySos;
            set => _enemySos = value;
        }

        public List<NpcSO> NpcSos
        {
            get => _npcSos;
            set => _npcSos = value;
        }

        public List<ItemSo> ItemSos
        {
            get => _itemSos;
            set => _itemSos = value;
        }

        public void Init()
        {
            graph = new List<QuestSO>();
            _dungeonFileSos = new List<DungeonFileSo>();
            _enemySos = new List<EnemySO>();
            _npcSos = new List<NpcSO>();
            _itemSos = new List<ItemSo>();
            DungeonParametersForQuestLine = new QuestDungeonsParameters();
            EnemyParametersForQuestLine = new QuestEnemiesParameters();
            _itemParametersForQuestLine = new QuestItemsParameters();
            _npcParametersForQuestLine = new QuestNpcsParameters();
        }

        public void CreateAsset(PlayerProfile.PlayerProfileCategory playerProfileCategory)
        {
            if ( graph[0].symbolType == "empty" )
                return;
#if UNITY_EDITOR
            // Define the JSON file extension
            const string extension = ".asset";

            Debug.Log("Quest Size: " + graph.Count);
            // Build the target path
            var target = "Assets";
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Experiment";
            var newFolder = Constants.SEPARATOR_CHARACTER + playerProfileCategory.ToString();
            if (!AssetDatabase.IsValidFolder(target + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += newFolder;
            var fileName = target+ Constants.SEPARATOR_CHARACTER +"Narrative_" + graph[0] + extension;
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            AssetDatabase.Refresh();
#endif
        }
    }
}
