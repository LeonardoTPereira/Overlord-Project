using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Overlord-Project/QuestSO", order = 0)]
    public class QuestSO : ScriptableObject, Symbol {
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
        [SerializeField] private QuestSO nextWhenSuccess;
        [SerializeField] private QuestSO nextWhenFailure;
        [SerializeField] private QuestSO previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [SerializeField] private ItemSo reward;
        private bool _canDrawNext;

        public QuestSO NextWhenSuccess { get => nextWhenSuccess; set => nextWhenSuccess = value; }
        public QuestSO NextWhenFailure { get => nextWhenFailure; set => nextWhenFailure = value; }
        public QuestSO Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public ItemSo Reward { get => reward; set => reward = value; }

        public virtual void Init()
        {
            nextWhenSuccess = null;
            nextWhenFailure = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Reward = null;
        }

        public void Init(string name, bool endsStoryLine, QuestSO previous)
        {
            QuestName = name;
            EndsStoryLine = endsStoryLine;
            Previous = previous;
            nextWhenSuccess = null;
            nextWhenFailure = null;
            Reward = null;
        }

        public void SetDictionary( Dictionary<string, Func<int,int>> _nextSymbolChances  )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        public void SetNextSymbol(MarkovChain chain)
        {
            int chance = (int) UnityEngine.Random.Range( 0, 100 );
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
        
        public void SaveAsAsset()
        {
            #if UNITY_EDITOR
            var target = "Assets";
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "Experiment";
            var newFolder = "Quests";
            if (!AssetDatabase.IsValidFolder(target + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SEPARATOR_CHARACTER + newFolder;
            target += Constants.SEPARATOR_CHARACTER;
            target += QuestName+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(target);
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
