using System;
using System.Collections.Generic;
using System.Linq;
using Game.NPCs;
using Game.Quests;
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
        [field: SerializeReference] public List<QuestSo> Quests {get; set; }
        [field: SerializeField] public NpcSo NpcInCharge { get; set; }
        [field: SerializeField] public int CurrentQuestIndex { get; set; }
        public static event QuestCompletedEvent QuestCompletedEventHandler;
        
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
                    CloseCurrentQuest();
                }

                return true;
            }
            return false;
        }

        private void CloseCurrentQuest()
        {
            QuestSo currentQuest;
            do
            {
                currentQuest = GetCurrentQuest();
                QuestCompletedEventHandler?.Invoke(null, new NewQuestEventArgs(currentQuest, NpcInCharge));
                currentQuest.IsClosed = true;
                CurrentQuestIndex++;
            } while (currentQuest.IsCompleted);
        }

        public QuestSo GetCurrentQuest()
        {
            return CurrentQuestIndex >= Quests.Count ? null : Quests[CurrentQuestIndex];
        }

        public void PopulateQuestLine(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            var questChain = new MarkovChain();
            while (questChain.GetLastSymbol().CanDrawNext)
            {
                var lastSelectedQuest = questChain.GetLastSymbol();
                lastSelectedQuest.NextSymbolChances = ProfileCalculator.StartSymbolWeights;
                lastSelectedQuest.SetNextSymbol(questChain);

                var nonTerminalSymbol = questChain.GetLastSymbol();
                nonTerminalSymbol.SetNextSymbol(questChain);
                questChain.GetLastSymbol().DefineQuestSo(Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes);
            }
        }
    }
}
