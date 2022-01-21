﻿// ﻿using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestTerminals;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Overlord-Project/QuestSO", order = 0)]
    public class QuestSO : ScriptableObject, Symbol {
        public virtual string symbolType {get; set;}
        public Dictionary<string, Func<float,float>> nextSymbolChances {get; set;}
        public virtual bool canDrawNext {
            get { return true; } 
            set {} 
        }
        private QuestSO nextWhenSuccess;
        private QuestSO nextWhenFailure;
        private QuestSO previous;
        private string questName;
        private bool endsStoryLine;
        private TreasureSO reward;

        public QuestSO NextWhenSuccess { get => nextWhenSuccess; set => nextWhenSuccess = value; }
        public QuestSO NextWhenFailure { get => nextWhenFailure; set => nextWhenFailure = value; }
        public QuestSO Previous { get => previous; set => previous = value; }
        public string QuestName { get => questName; set => questName = value; }
        public bool EndsStoryLine { get => endsStoryLine; set => endsStoryLine = value; }
        public TreasureSO Reward { get => reward; set => reward = value; }

        public virtual void Init()
        {
            nextWhenSuccess = null;
            nextWhenFailure = null;
            previous = null;
            questName = "Null";
            endsStoryLine = false;
            Reward = null;
        }

        public virtual void Init(string name, bool endsStoryLine, QuestSO previous)
        {
            QuestName = name;
            EndsStoryLine = endsStoryLine;
            Previous = previous;
            nextWhenSuccess = null;
            nextWhenFailure = null;
            Reward = null;
        }

        public void SaveAsAsset(string assetName)
        {
            AssetDatabase.CreateAsset(this, assetName+".asset");
        }

        void Symbol.SetDictionary( Dictionary<string, Func<float,float>> _nextSymbolChances  )
        {
            nextSymbolChances = _nextSymbolChances;
        }

        void Symbol.SetNextSymbol(MarkovChain chain)
        {
            float chance = (float) UnityEngine.Random.Range( 0, 100 ) / 100 ;
            float cumulativeProbability = 0;
            foreach ( KeyValuePair<string, Func<float,float>> nextSymbolChance in nextSymbolChances )
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
        
        public bool IsItemQuest()
        {
            return GetType().IsAssignableFrom(typeof(ItemQuestSO));
        }
        
        public bool IsDropQuest()
        {
            return GetType().IsAssignableFrom(typeof(DropQuestSO));
        }

        public bool IsKillQuest()
        {
            return GetType().IsAssignableFrom(typeof(KillQuestSO));
        }        
        
        public bool IsGetQuest()
        {
            return GetType().IsAssignableFrom(typeof(GetQuestSO));
        }        
        public bool IsSecretRoomQuest()
        {
            return GetType().IsAssignableFrom(typeof(SecretRoomQuestSO));
        }
        
        public bool IsExplorationQuest()
        {
            return IsSecretRoomQuest() || IsGetQuest();
        }
        
        public bool IsTalkQuest()
        {
            return GetType().IsAssignableFrom(typeof(TalkQuestSO));
        }
    }
}
