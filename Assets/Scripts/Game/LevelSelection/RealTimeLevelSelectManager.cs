using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.Events;
using Game.SaveLoadSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util;

namespace Game.LevelSelection
{
    public class RealTimeLevelSelectManager : LevelSelectManager
    {
        [SerializeField] private bool _automaticalySelectIdealProfile = false;              // If true, after the player answers the pre-test, it auto jump to ideal profile game
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

            if (_automaticalySelectIdealProfile)
            {
                int aw, cw, iw, mw;

                iw = PlayerPrefs.GetInt("ImmersionWeight");
                aw = PlayerPrefs.GetInt("AchievementWeight");
                mw = PlayerPrefs.GetInt("MasteryWeight");
                cw = PlayerPrefs.GetInt("CreativityWeight");
                Debug.Log("IW:" + iw + " AW:" + aw + " MW:" + mw + " CW:" + cw);

                foreach (LevelSelectItem levelItem in LevelItems)
                {
                    
                    if (!levelItem.Level.HasCompleted() && levelItem.Level.IsSameProfile(aw, cw, iw, mw))
                    {
                        if (levelItem.Level is RealTimeLevelData realTimeLevelData)
                        {
                            Selected.SelectLevel(levelItem.Level);
                            var profileWeights = new List<int>();

                            profileWeights.Add(aw);
                            profileWeights.Add(cw);
                            profileWeights.Add(iw);
                            profileWeights.Add(mw);
                            PreTestFormQuestionAnsweredEventHandler?.Invoke(this, new FormAnsweredEventArgs(-1, profileWeights));
                            switch (settings.GameType)
                            {
                                case Enums.GameType.TopDown:
                                    SceneManager.LoadScene("ContentGenerator");
                                    break;
                                case Enums.GameType.Platformer:
                                    SceneManager.LoadScene("PlatformContentGenerator");
                                    break;
                            }
                            return;
                        }
                        Debug.LogError("Level Data is not Real Time!");
                        return;
                    }
                }
            }
        }

        protected override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "LevelSelector") return;
            if (AllLevelsCompleted())
            {
                InvokeCompletedLevelsEvent();

                switch (settings.GameType)
                {
                    case Enums.GameType.TopDown:
                        SceneManager.LoadScene("ContentGenerator");
                        break;
                    case Enums.GameType.Platformer:
                        SceneManager.LoadScene("PlatformContentGenerator");
                        break;
                }
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
                    switch (settings.GameType)
                    {
                        case Enums.GameType.TopDown:
                            SceneManager.LoadScene("ContentGenerator");
                            break;
                        case Enums.GameType.Platformer:
                            SceneManager.LoadScene("PlatformContentGenerator");
                            break;
                    }
                    return;
                }
                Debug.LogError("Level Data is not Real Time!");
                return;
            }
        }
    }
}