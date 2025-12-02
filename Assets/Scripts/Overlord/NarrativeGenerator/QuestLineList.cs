using System;
using System.Collections.Generic;
using Overlord.LevelGenerator.LevelSOs;
using Overlord.ProfileAnalyst;
using Overlord.NarrativeGenerator.EnemyRelatedNarrative;
using Overlord.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;
using UnityEngine;
using Util;
using System.Linq;
using Overlord.NarrativeGenerator.NPCs;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Overlord.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLineList", menuName = "Overlord-Project/QuestLineList", order = 0)]
    [Serializable]
    public class QuestLineList : ScriptableObject, ISavableGeneratedContent
    {
        [field: SerializeField] public List<QuestLine> QuestLines { get; set; }
        [field: SerializeField] public List<EnemySO> EnemySos { get; set; }
        [field: SerializeField] public List<NpcSo> NpcSos { get; set; }
        [field: SerializeField] public List<ItemSo> ItemSos { get; set; }
        [field: SerializeField] public List<DungeonFileSo> DungeonFileSos { get; set; }


        [field: SerializeField] public QuestItemsParameters ItemParametersForQuestLines { get; set; }
        [field: SerializeField] public QuestDungeonsParameters DungeonParametersForQuestLines { get; set; }
        [field: SerializeField] public QuestEnemiesParameters EnemyParametersForQuestLines { get; set; }
        [field: SerializeField] public YeePlayerProfile TargetProfile { get; set; }

        public void Init()
        {
            QuestLines = new List<QuestLine>();
            DungeonFileSos = new List<DungeonFileSo>();
            EnemySos = new List<EnemySO>();
            NpcSos = new List<NpcSo>();
            ItemSos = new List<ItemSo>();
            DungeonParametersForQuestLines = new QuestDungeonsParameters();
            EnemyParametersForQuestLines = new QuestEnemiesParameters();
            ItemParametersForQuestLines = new QuestItemsParameters();
        }

        public void Init(QuestLineList copiedQuestLineList)
        {
            DungeonFileSos = copiedQuestLineList.DungeonFileSos;
            EnemySos = copiedQuestLineList.EnemySos;
            NpcSos = copiedQuestLineList.NpcSos;
            ItemSos = copiedQuestLineList.ItemSos;
            DungeonParametersForQuestLines = copiedQuestLineList.DungeonParametersForQuestLines;
            EnemyParametersForQuestLines = copiedQuestLineList.EnemyParametersForQuestLines;
            ItemParametersForQuestLines = copiedQuestLineList.ItemParametersForQuestLines;
            QuestLines = new List<QuestLine>();
            foreach (var questLine in copiedQuestLineList.QuestLines)
            {
                var copyQuestLine = CreateInstance<QuestLine>();
                copyQuestLine.Init(questLine);
                AddQuestLine(copyQuestLine);
            }
        }

        public void AddQuestLine(QuestLine questLine)
        {
            QuestLines.Add(questLine);
        }

        public QuestLine GetRandomQuestLine()
        {
            var random = RandomSingleton.GetInstance().Random;
            return QuestLines[random.Next(QuestLines.Count)];
        }
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string questLineName = "QuestLineList";
            var fileName = directory + questLineName + ".asset";
            if (!AssetDatabase.GUIDFromAssetPath(fileName).Empty()) return;
            CreateAssetsForDungeons(directory);
            CreateAssetsForEnemies(directory);
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            var newFolder = Constants.SeparatorCharacter + TargetProfile.ToString();
            if (!AssetDatabase.IsValidFolder(directory + newFolder))
            {
                AssetDatabase.CreateFolder(directory, newFolder);
            }
            directory += Constants.SeparatorCharacter + newFolder;
            foreach (var questLine in QuestLines)
            {
                questLine.SaveAsset(directory);
            }
#endif
        }
        
        public void CreateAssetsForDungeons(string directory)
        {
            foreach (var dungeon in DungeonFileSos)
            {
                dungeon.SaveAsset(directory);
            }
        }
        
        public void CreateAssetsForEnemies(string directory)
        {
            foreach (var enemy in EnemySos)
            {
                enemy.SaveAsset(directory);
            }
        }

        public void CalculateDifficultyFromProfile(float masteryPreference)
        {
            EnemyParametersForQuestLines.CalculateDifficultyFromProfile(masteryPreference/100f);
        }

        public void CalculateItemsFromQuests()
        {
            foreach (var quest in QuestLines.SelectMany(QuestLines => QuestLines.Quests))
            {
                ItemParametersForQuestLines.CalculateItemsFromQuests(quest);
            }
        }

        public void CalculateMonsterFromQuests()
        {
            foreach (var quest in QuestLines.SelectMany(QuestLines => QuestLines.Quests))
            {
                EnemyParametersForQuestLines.CalculateMonsterFromQuests(quest);
            }
        }

        public void CalculateDungeonParametersFromQuests(float explorationPreference, float achievementPreference)
        {
            DungeonParametersForQuestLines.CalculateDungeonParametersFromQuests(QuestLines
                , explorationPreference/100f, achievementPreference/100f);
        }      

        public void SetRandomMainQuest(List<int> rewardedKeys)
        {
            List<QuestLine> mainQuestLines = QuestLines.FindAll( x => x.IsMainQuest );
            foreach (QuestLine questLine in mainQuestLines)
            {
                questLine.IsMainQuest = false;
            }

            QuestLine mainQuestLine = GetRandomQuestLine();
            mainQuestLine.SetAsMainQuestLine(rewardedKeys);
        }

        public void OpenStartingQuests()
        {
            foreach (var questLine in QuestLines)
            {
                questLine.OpenCurrentQuest();
            }
        }

        public void ConvertDataForCurrentDungeon(List<DungeonRoomData> dungeonParts)
        {
            foreach (var questLine in QuestLines)
            {
                questLine.ConvertDataForCurrentDungeon(dungeonParts);
            }
        }
    }
}