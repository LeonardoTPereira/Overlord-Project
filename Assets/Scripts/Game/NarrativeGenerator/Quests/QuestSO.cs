using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Overlord-Project/QuestSO", order = 0)]
    [Serializable]
    public class QuestSO : ScriptableObject, SaveableGeneratedContent, Symbol
    {
        public virtual string symbolType {get; set;}
        public virtual Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get;
            set;
        }
        public virtual bool canDrawNext {
            get { return true; } 
            set {} 
        }

        [SerializeReference] private QuestSO next;
        [SerializeReference] private QuestSO previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [SerializeField] private ItemSo reward;
        [field: SerializeField] public bool IsCompleted { get; set; }
        private bool _canDrawNext;

        public QuestSO Next { get => next; set => next = value; }
        public QuestSO Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public ItemSo Reward { get => reward; set => reward = value; }

        public virtual void Init()
        {
            next = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Reward = null;
        }

        public void Init(string questTitle, bool endsLine, QuestSO previousQuest)
        {
            QuestName = questTitle;
            EndsStoryLine = endsLine;
            Previous = previousQuest;
            next = null;
            Reward = null;
        }
        
        public virtual void Init(QuestSO copiedQuest)
        {
            QuestName = copiedQuest.QuestName;
            EndsStoryLine = copiedQuest.EndsStoryLine;
            Previous = copiedQuest.Previous;
            next = copiedQuest.Next;
            Reward = copiedQuest.Reward;
        }

        public virtual QuestSO Clone()
        {
            var cloneQuest = CreateInstance<QuestSO>();
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
            foreach ( KeyValuePair<string, Func<int,int>> nextSymbolChance in nextSymbolChances )
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
            return typeof(KillQuestSO).IsAssignableFrom(GetType());
        }        
        
        public bool IsGetQuest()
        {
            return typeof(GetQuestSo).IsAssignableFrom(GetType());
        }        
        public bool IsSecretRoomQuest()
        {
            return typeof(SecretRoomQuestSO).IsAssignableFrom(GetType());
        }
        
        public bool IsExplorationQuest()
        {
            return IsSecretRoomQuest() || IsGetQuest();
        }
        
        public bool IsTalkQuest()
        {
            return typeof(TalkQuestSO).IsAssignableFrom(GetType());
        }
    }
}
