using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSo", menuName = "Overlord-Project/QuestSo", order = 0)]
    [Serializable]
    public class QuestSo : ScriptableObject, SaveableGeneratedContent, Symbol
    {
        public virtual string symbolType {get; set;}
        public virtual Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get { return nextSymbolChances; }
            set {}
        }
        protected Dictionary<string, Func<int,int>> nextSymbolChances;
        public virtual bool canDrawNext {
            get { return true; } 
            set {} 
        }

        [SerializeReference] private QuestSo next;
        [SerializeReference] private QuestSo previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [SerializeField] private ItemSo reward;
        [field: SerializeField] public bool IsCompleted { get; set; }
        private bool _canDrawNext;

        public QuestSo Next { get => next; set => next = value; }
        public QuestSo Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public ItemSo Reward { get => reward; set => reward = value; }
        public int Id { get; set; }

        public virtual void DefineQuestSo ( MarkovChain chain, List<QuestSo> QuestSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
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
        
        public void SetDictionary(Dictionary<string, Func<int,int>> _nextSymbolChances  )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        public void SetNextSymbol(MarkovChain chain)
        {
            int chance = RandomSingleton.GetInstance().Next(0, 100);
            int cumulativeProbability = 0;
            foreach ( KeyValuePair<string, Func<int,int>> _nextSymbolChance in NextSymbolChances )
            {
                cumulativeProbability += _nextSymbolChance.Value( chain.symbolNumber );
                if ( cumulativeProbability >= chance )
                {
                    string nextSymbol = _nextSymbolChance.Key;
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
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SEPARATOR_CHARACTER + newFolder;
            fileName += Constants.SEPARATOR_CHARACTER;
            fileName += QuestName+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            #endif
        }

        public bool IsItemQuest()
        {
            return typeof(ItemQuestSo).IsAssignableFrom(GetType());
        }
        
        public bool IsDropQuest()
        {
            return typeof(DropQuestSo).IsAssignableFrom(GetType());
        }

        public bool IsKillQuest()
        {
            return typeof(KillQuestSo).IsAssignableFrom(GetType());
        }        
        
        public bool IsGetQuest()
        {
            return typeof(GetQuestSo).IsAssignableFrom(GetType());
        }        
        public bool IsSecretRoomQuest()
        {
            return typeof(SecretRoomQuestSo).IsAssignableFrom(GetType());
        }
        
        public bool IsExplorationQuest()
        {
            return IsSecretRoomQuest() || IsGetQuest();
        }
        
        public bool IsTalkQuest()
        {
            return typeof(ListenQuestSo).IsAssignableFrom(GetType());
        }
    }
}
