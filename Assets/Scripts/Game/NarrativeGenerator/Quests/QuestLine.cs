using System;
using System.Collections.Generic;
using System.Linq;
using Game.ExperimentControllers;
using Game.LevelGenerator;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using Game.Quests;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Overlord-Project/QuestLine", order = 0)]
    [Serializable]
    public class QuestLine : ScriptableObject, ISavableGeneratedContent
    {
        [field: SerializeReference] public List<QuestSo> Quests {get; set; }
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }
        public static event QuestCompletedEvent QuestCompletedEventHandler;
        public static event QuestOpenedEvent QuestOpenedEventHandler;

        public void Init()
        {
            Quests = new List<QuestSo>();
        }

        public void Init(QuestLine questLine)
        {
            Quests = new List<QuestSo>();
            foreach (var copyQuest in questLine.Quests.Select(quest => quest.Clone()))
            {
                if (Quests.Count > 0)
                {
                    Quests[^1].Next = copyQuest;
                    copyQuest.Previous = Quests[^1];
                }
                Quests.Add(copyQuest);
            }
            NpcInCharge = questLine.NpcInCharge;
        }
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            var newDirectory = Constants.SeparatorCharacter + "QuestLine";
            var guid = AssetDatabase.CreateFolder(directory, newDirectory);
            newDirectory = AssetDatabase.GUIDToAssetPath(guid);
            CreateAssetsForQuests(newDirectory);
            const string extension = ".asset";
            var fileName = newDirectory+ Constants.SeparatorCharacter +"Narrative_" + Quests[0] + extension;
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            AssetDatabase.Refresh();
#endif
        }
        
        public void CreateAssetsForQuests(string directory)
        {
            foreach (var quest in Quests)
            {
                quest.SaveAsset(directory);
            }
        }

        public bool RemoveAvailableQuestWithId<T, U>(U questElement, int questId) where T : QuestSo
        {
            foreach (var quest in Quests)
            {
                if (quest is not T questSo) continue;
                if (!questSo.HasAvailableElementWithId(questElement, questId)) continue;
                questSo.RemoveElementWithId(questElement, questId);
                if (questSo.IsCompleted && questSo == GetCurrentQuest())
                {
                    CompleteCurrentQuest();
                }
                if(quest is not ExploreQuestSo && quest is not GotoQuestSo) return true;
            }
            return false;
        }

        private void CompleteCurrentQuest()
        {
            var currentQuest = GetCurrentQuest();
            QuestCompletedEventHandler?.Invoke(null, new NewQuestEventArgs(currentQuest, NpcInCharge));
        }
        
        public void CloseCurrentQuest()
        {
            GetCurrentQuest().IsClosed = true;
            CurrentQuestIndex++;
            if (GetCurrentQuest() == null) return;
            OpenCurrentQuest();
        }

        public void OpenCurrentQuest()
        {
            var quest = GetCurrentQuest();
            QuestOpenedEventHandler?.Invoke(null, new NewQuestEventArgs(quest, NpcInCharge));
            if (!quest.IsCompleted) return;
            CompleteCurrentQuest();
        }

        public QuestSo GetCurrentQuest()
        {
            return CurrentQuestIndex >= Quests.Count ? null : Quests[CurrentQuestIndex];
        }

        public List<QuestSo> GetCompletedQuests()
        {
            List<QuestSo> completedQuests = new List<QuestSo>();
            for (int i = 0; i < CurrentQuestIndex; i++)
            {
                completedQuests.Add( Quests[i] );
            }
            return completedQuests; 
        }

        public void PopulateQuestLine(in GeneratorSettings generatorSettings)
        {
            var questChain = new MarkovChain();
            while (questChain.GetLastSymbol().CanDrawNext)
            {
                var lastSelectedQuest = questChain.GetLastSymbol();
                lastSelectedQuest.NextSymbolChances = ProfileCalculator.StartSymbolWeights;
                lastSelectedQuest.SetNextSymbol(questChain);

                var nonTerminalSymbol = questChain.GetLastSymbol();
                nonTerminalSymbol.SetNextSymbol(questChain);
                questChain.GetLastSymbol().DefineQuestSo(Quests, in generatorSettings);
            }
        }

        public void ConvertDataForCurrentDungeon(List<DungeonRoomData> dungeonParts)
        {
            foreach (var quest in Quests)
            {
                switch (quest)
                {
                    case ExploreQuestSo exploreQuest:
                        var roomCount = dungeonParts.Count(room => room.Type != Constants.RoomTypeString.Corridor);
                        exploreQuest.ChangeRoomsPercentageToValue(roomCount);
                        break;
                    case GotoQuestSo gotoQuest:
                        gotoQuest.SelectRoomCoordinates(dungeonParts);
                        break;
                }
                quest.CreateQuestString();
            }
        }
    }
}
