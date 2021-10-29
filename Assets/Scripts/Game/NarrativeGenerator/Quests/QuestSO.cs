using Game.NarrativeGenerator.Quests.QuestTerminals;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
namespace Game.NarrativeGenerator.Quests
{
    public abstract class QuestSO : ScriptableObject
    {
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
