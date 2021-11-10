// ﻿using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using ScriptableObjects;
using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestTerminals;

namespace Game.NarrativeGenerator.Quests
{
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Overlord-Project/QuestSO", order = 0)]
    public class QuestSO : ScriptableObject {
        public bool canDrawNext
        {
            get => _canDrawNext;
            set => _canDrawNext = value;
        }

        [SerializeField] private QuestSO nextWhenSuccess;
        [SerializeField] private QuestSO nextWhenFailure;
        [SerializeField] private QuestSO previous;
        [SerializeField] private string questName;
        [SerializeField] private bool endsStoryLine;
        [SerializeField] private TreasureSO reward;
        private bool _canDrawNext;

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
