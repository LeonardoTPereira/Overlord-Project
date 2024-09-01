using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.Events;
using Game.SaveLoadSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class RealTimeLevelSelectManager : LevelSelectManager
    {
        public static event FormAnsweredEvent PreTestFormQuestionAnsweredEventHandler;
        public static event Action SaveStateHandler;
        private bool _hasPressedButton;

        private void Awake()
        {
            _hasPressedButton = false;
        }

        private void Start()
        {

            SaveStateHandler?.Invoke();
        }

        protected override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "LevelSelector") return;
            if (AllLevelsCompleted())
            {
                InvokeCompletedLevelsEvent();
                SceneManager.LoadScene("ContentGenerator");
            }
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.LevelSelectTheme));
        }
        
        public override void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            if(_hasPressedButton) return;
            foreach (var levelItem in LevelItems.Where(levelItem => levelItem.IsSelected))
            {
                _hasPressedButton = true;
                Selected.SelectLevel(levelItem.Level);
                var profileWeights = new List<int>();
                if (levelItem.Level is RealTimeLevelData realTimeLevelData)
                {
                    profileWeights.Add(realTimeLevelData.AchievementWeight);
                    profileWeights.Add(realTimeLevelData.CreativityWeight);
                    profileWeights.Add(realTimeLevelData.ImmersionWeight);
                    profileWeights.Add(realTimeLevelData.MasteryWeight);
                    PreTestFormQuestionAnsweredEventHandler?.Invoke(this, new FormAnsweredEventArgs(-1, profileWeights));
                    SceneManager.LoadScene("ContentGenerator");
                    return;
                }
                Debug.LogError("Level Data is not Real Time!");
                return;
            }
        }
    }
}