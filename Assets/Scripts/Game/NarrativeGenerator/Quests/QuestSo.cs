using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEngine;
using Util;
using Game.NPCs;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSo", menuName = "Overlord-Project/QuestSo", order = 0)]
    [Serializable]
    public abstract class QuestSo : ScriptableObject, SaveableGeneratedContent, ISymbol
    {
        public virtual string SymbolType {get; set;}
        public virtual Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get => nextSymbolChances;
            set {}
        }
        protected Dictionary<string, Func<int,int>> nextSymbolChances;
        public virtual bool CanDrawNext {
            get => true;
            set {} 
        }

        [SerializeReference] private QuestSo next;
        [SerializeReference] private QuestSo previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [SerializeField] private ItemSo reward;
        [field: SerializeField] public bool IsCompleted { get; set; }
        [field: SerializeField] public bool IsClosed { get; set; }
        private bool _canDrawNext;

        public QuestSo Next { get => next; set => next = value; }
        public QuestSo Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public ItemSo Reward { get => reward; set => reward = value; }
        public int Id { get; set; }

        public virtual void DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
        }

        public virtual void Init()
        {
            next = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Reward = null;
            Id = GetInstanceID();
        }

        public void Init(string questTitle, bool endsLine, QuestSo previousQuest)
        {
            QuestName = questTitle;
            EndsStoryLine = endsLine;
            Previous = previousQuest;
            next = null;
            Reward = null;
            Id = GetInstanceID();
        }
        
        public virtual void Init(QuestSo copiedQuest)
        {
            QuestName = copiedQuest.QuestName;
            EndsStoryLine = copiedQuest.EndsStoryLine;
            Previous = copiedQuest.Previous;
            next = copiedQuest.Next;
            Reward = copiedQuest.Reward;
            Id = copiedQuest.Id;
        }

        public virtual QuestSo Clone()
        {
            var cloneQuest = CreateInstance<QuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }
        
        public void SetDictionary(Dictionary<string, Func<int,int>> nextSymbolChances  )
        {
            this.nextSymbolChances = nextSymbolChances;
        }

        public void SetNextSymbol(MarkovChain chain)
        {
            int chance = RandomSingleton.GetInstance().Next(0, 100);
            int cumulativeProbability = 0;
            foreach ( KeyValuePair<string, Func<int,int>> nextSymbolChance in NextSymbolChances )
            {
                cumulativeProbability += nextSymbolChance.Value( chain.symbolNumber );
                if ( cumulativeProbability >= chance )
                {
                    string nextSymbol = nextSymbolChance.Key;
                    chain.SetSymbol( nextSymbol );
                    break;
                }
            }
        }

        public void SaveAsset(string directory)
        {
            #if UNITY_EDITOR
            const string newFolder = "Quests";
            var fileName = directory;
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SeparatorCharacter + newFolder;
            fileName += Constants.SeparatorCharacter;
            fileName += QuestName+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            #endif
        }

        public bool IsKillQuest()
        {
            return typeof(KillQuestSo).IsAssignableFrom(GetType());
        }

        
        public bool IsTalkQuest()
        {
            return typeof(ListenQuestSo).IsAssignableFrom(GetType());
        }

        public abstract bool HasAvailableElementWithId<T>(T questElement, int questId);
        public abstract void RemoveElementWithId<T>(T questElement, int questId);
    }
}
