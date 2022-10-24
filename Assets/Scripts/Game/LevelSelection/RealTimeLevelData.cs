using System;
using Game.SaveLoadSystem;
using UnityEngine;

namespace Game.LevelSelection
{
    [Serializable]
    [CreateAssetMenu(fileName = "RealTimeLevelData", menuName = "Overlord-Project/RealTimeLevelData", order = 0)]
    public class RealTimeLevelData : LevelData, ISaveable
    { 
        [field: SerializeField] public int AchievementWeight { get; set; }
        [field: SerializeField] public int CreativityWeight { get; set; }
        [field: SerializeField] public int ImmersionWeight { get; set; }
        [field: SerializeField] public int MasteryWeight { get; set; }
        public object SaveState()
        {
            return new SaveData()
            {
                Completed = IsCompleted,
                Surrendered = HasSurrendered
            };
        }

        public void LoadState(object state)
        {
            var saveData = (SaveData) state;
            IsCompleted = saveData.Completed;
            HasSurrendered = saveData.Surrendered;
            HasDataBeenLoaded = true;
        }

        [Serializable]
        private struct SaveData
        {
            [SerializeField] public bool Completed;
            [SerializeField] public bool Surrendered;
        }
    }
}