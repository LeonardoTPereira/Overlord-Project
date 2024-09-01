using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.ExperimentControllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util;

namespace Game.LevelSelection
{
    public class LevelSelectManager : MonoBehaviour, ISoundEmitter
    {
        [field: SerializeField] public SelectedLevels Selected { get; set; }
        [field: SerializeField] public List<LevelSelectItem> LevelItems { get; set; }

        [field: SerializeField] public GeneratorSettings settings;

        public static event EventHandler CompletedAllLevelsEventHandler;
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "LevelSelector") return;
            if (AllLevelsCompleted())
            {
                InvokeCompletedLevelsEvent();
                SceneManager.LoadScene("ContentGenerator");
            }
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.LevelSelectTheme));
        }

        protected static void InvokeCompletedLevelsEvent()
        {
            CompletedAllLevelsEventHandler?.Invoke(null, EventArgs.Empty);
        }

        protected bool AllLevelsCompleted()
        {
            return LevelItems.All(level => level.Level.HasCompleted());
        }

        public virtual void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            foreach (var levelItem in LevelItems.Where(levelItem => levelItem.IsSelected))
            {

                Selected.SelectLevel(levelItem.Level);

                switch (settings.GameType)
                {
                    case Enums.GameType.Platformer: SceneManager.LoadScene("Dungeon");
                        break;
                    case Enums.GameType.TopDown: SceneManager.LoadScene("LevelWithEnemies");
                        break;
                }

                return;
            }
        }
    }
    

}