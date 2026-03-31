using System;
using System.Collections.Generic;
using System.Linq;
//using Game.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Util;
using static Util.Enums;
using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Events;
using Overlord.ProfileAnalyst;

namespace Overlord.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Overlord-Project/QuestLine", order = 0)]
    [Serializable]
    public class QuestLine : ScriptableObject, ISavableGeneratedContent
    {
        [field: SerializeReference] public List<QuestSo> Quests {get; set; }
        [field: SerializeReference] public bool IsMainQuest {get; set; }
        [field: SerializeReference] public List<int> RewardKeys = new List<int>();
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }

        public static event QuestLineCompletedEvent QuestLineCompletedEventHandler;
        public static event QuestLineOpenedEvent QuestLineOpenedEventHandler;

        public static event QuestCompletedEvent QuestCompletedEventHandler;
        public static event QuestOpenedEvent QuestOpenedEventHandler;
        public static event QuestElementEvent AllowExchangeEventHandler;
        // Check point dialogue, might not be the best name but I'm sleepy
        public static event QuestElementEvent AllowCheckPointEventHandler;
        public static event QuestElementEvent AllowGiveEventHandler;

        protected Language _language;

        public void Init()
        {
            Quests = new List<QuestSo>();
            CurrentQuestIndex = 0;
        }

        public void Init(Language language)
        {
            Init();
            _language = language;
        }

        public void Init(QuestLine questLine)
        {
            Init();
            foreach (var copyQuest in questLine.Quests.Select(quest => quest.Clone()))
            {
                if (Quests.Count > 0)
                {
                    Quests[^1].Next = copyQuest;
                    copyQuest.Previous = Quests[^1];
                }
                Quests.Add(copyQuest);
            }
            IsMainQuest = questLine.IsMainQuest;

            RewardKeys = new List<int>();
            RewardKeys.AddRange(questLine.RewardKeys);

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
            var fileName = newDirectory + Constants.SeparatorCharacter + "Narrative_" + Quests[0] + extension;
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

                switch (questSo)
                {
                    case ExchangeQuestSo { HasItems: true, IsCompleted: false, HasCreatedDialogue: false } exchangeQuestSo:
                        exchangeQuestSo.HasCreatedDialogue = true;
                        AllowExchangeEventHandler?.Invoke(null, new QuestExchangeEventArgs(exchangeQuestSo));
                        break;
                    case GiveQuestSo { HasItem: true, IsCompleted: false, HasCreatedDialogue: false } giveQuestSo:
                        giveQuestSo.HasCreatedDialogue = true;
                        AllowGiveEventHandler?.Invoke(null, new QuestGiveEventArgs(giveQuestSo));
                        break;
                }

                if (quest is not ExploreQuestSo && quest is not GotoQuestSo) return true;
            }
            return false;
        }

        public void CompleteCurrentQuest()
        {
            Debug.Log("complete current quest");
            QuestCompletedEventHandler?.Invoke(null, new NewQuestEventArgs(GetCurrentQuest(), NpcInCharge));
            if ( CurrentQuestIndex+1 >= Quests.Count )
            {
                Debug.Log("invoke questline completion");                    
                QuestLineCompletedEventHandler?.Invoke(null, new NewQuestLineEventArgs(this));
            }
        }

        public void CloseCurrentQuest()
        {
            GetCurrentQuest().IsClosed = true;
            CurrentQuestIndex++;
            if (GetCurrentQuest() != null)
            {
                OpenCurrentQuest();
            }
        }

        public void SetAsMainQuestLine( List<int> rewardedKeys )
        {
            IsMainQuest = true;
            RewardKeys = new List<int>();
            RewardKeys.AddRange( rewardedKeys );
            foreach (var key in rewardedKeys)
            {
                Debug.Log(key);
            }
        }

        public void OpenCurrentQuest()
        {
            var quest = GetCurrentQuest();
            if ( CurrentQuestIndex == 0 && IsMainQuest)
            {
                QuestLineOpenedEventHandler?.Invoke(null, new NewQuestLineEventArgs(this));
            }
            QuestOpenedEventHandler?.Invoke(null, new NewQuestEventArgs(quest, NpcInCharge));
            quest.IsOpened = true;
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
                completedQuests.Add(Quests[i]);
            }
            return completedQuests;
        }

        public void PopulateQuestLine(in NarrativeSettings narrativeSettings, NpcSo npcInCharge )
        {
            Dictionary<string, Func<int, float>> startSymbolWeights = YeeProfileCalculator.StartSymbolWeights;

            if (PlayerProfileManager.GetRandomProfile)
            {
                startSymbolWeights = GetRandomSymbolWeights();
            }
            PopulateQuestLineMarkov(narrativeSettings, npcInCharge, startSymbolWeights);
        }

        protected void PopulateQuestLineMarkov(in NarrativeSettings narrativeSettings, NpcSo npcInCharge, Dictionary<string, Func<int, float>> startSymbolWeights )
        {
            var questChain = new MarkovChain();
            while (questChain.GetLastSymbol().CanDrawNext)
            {
                var lastSelectedQuest = questChain.GetLastSymbol();
                lastSelectedQuest.NextSymbolChances = startSymbolWeights;
                lastSelectedQuest.SetNextSymbol(questChain);

                var nonTerminalSymbol = questChain.GetLastSymbol();
                nonTerminalSymbol.SetNextSymbol(questChain);
                questChain.GetLastSymbol().DefineQuestSo(Quests, npcInCharge, in narrativeSettings, _language);
            }
        }

        public void CompleteMissingQuests(in NarrativeSettings narrativeSettings, NpcSo npcInCharge, Dictionary<string,bool> addedQuests )
        {
            List<string> missingQuests = new List<string>();
            foreach (KeyValuePair<string, bool> quest in addedQuests)
            {
                if (!quest.Value)
                    missingQuests.Add(quest.Key);
            }

            var questChain = new MarkovChain();
            foreach (string missingQuest in missingQuests)
            {
                questChain.SetSymbol(missingQuest);
                questChain.GetLastSymbol().DefineQuestSo(Quests, npcInCharge, in narrativeSettings, _language);
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
                quest.CreateQuestString(_language);
            }
        }

        protected Dictionary<string, Func<int, float>> GetRandomSymbolWeights()
        {
            return new Dictionary<string, Func<int, float>>
            {
                {Constants.ImmersionQuest, _ => 25f},
                {Constants.AchievementQuest, _ => 25f},
                {Constants.MasteryQuest, _ => 25f},
                {Constants.CreativityQuest, _ => 25f}
            };
        }
    }
}