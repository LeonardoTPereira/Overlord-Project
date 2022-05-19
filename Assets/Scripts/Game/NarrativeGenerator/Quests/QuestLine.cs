using System;
using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Overlord-Project/QuestLine", order = 0)]
    [Serializable]
    public class QuestLine : ScriptableObject, SaveableGeneratedContent
    {
        [SerializeField] public List<QuestSO> graph;
        [SerializeField] private List<DungeonFileSo> _dungeonFileSos;
        [SerializeField] private QuestNpcsParameters _npcParametersForQuestLine;
        [SerializeField] private QuestItemsParameters _itemParametersForQuestLine;
        [SerializeField] private List<EnemySO> _enemySos;
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
            _itemSos = new List<ItemSo>();
            DungeonParametersForQuestLine = new QuestDungeonsParameters();
            EnemyParametersForQuestLine = new QuestEnemiesParameters();
            _itemParametersForQuestLine = new QuestItemsParameters();
            _npcParametersForQuestLine = new QuestNpcsParameters();
        }

        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            var newDirectory = Constants.SEPARATOR_CHARACTER + "QuestLine";
            var guid = AssetDatabase.CreateFolder(directory, newDirectory);
            newDirectory = AssetDatabase.GUIDToAssetPath(guid);
            CreateAssetsForQuests(newDirectory);
            CreateAssetsForDungeons(newDirectory);
            CreateAssetsForEnemies(newDirectory);
            const string extension = ".asset";
            var fileName = newDirectory+ Constants.SEPARATOR_CHARACTER +"Narrative_" + graph[0] + extension;
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            AssetDatabase.Refresh();
#endif
        }

        public void CreateAssetsForQuests(string directory)
        {
            foreach (var quest in graph)
            {
                quest.SaveAsset(directory);
            }
        }
        public void CreateAssetsForDungeons(string directory)
        {
            foreach (var dungeon in _dungeonFileSos)
            {
                dungeon.SaveAsset(directory);
            }
        }
        
        public void CreateAssetsForEnemies(string directory)
        {
            foreach (var enemy in _enemySos)
            {
                enemy.SaveAsset(directory);
            }
        }
        
    }
}
