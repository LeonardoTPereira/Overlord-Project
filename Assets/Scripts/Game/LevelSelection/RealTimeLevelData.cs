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

        public override void CompleteLevel()
        {
            Debug.Log("COMPLETED LEVEL: " + PlayerPrefsKey());
            PlayerPrefs.SetInt(PlayerPrefsKey(), 1);
            IsCompleted = true;
        }

        public override void GiveUpLevel()
        {
            Debug.Log("GIVEUP LEVEL: " + PlayerPrefsKey());
            PlayerPrefs.SetInt(PlayerPrefsKey(), 2);
            HasSurrendered = true;
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

        public override bool HasCompleted()
        {
            return IsCompleted || HasSurrendered || PlayerPrefs.HasKey(PlayerPrefsKey());
        }

        private string PlayerPrefsKey()
        {
            return ("A" + AchievementWeight + "C" + CreativityWeight + "I" + ImmersionWeight + "M" + MasteryWeight);
        }
    }
}