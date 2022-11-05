using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class LevelSelectManager : MonoBehaviour, ISoundEmitter
    {
        [field: SerializeField] public SelectedLevels Selected { get; set; }
        [field: SerializeField] public List<LevelSelectItem> LevelItems { get; set; }

        public static event EventHandler CompletedAllLevelsEventHandler;
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "LevelSelector") return;
            if (AllLevelsCompleted())
            {
                CompletedAllLevelsEventHandler?.Invoke(null, EventArgs.Empty);
                SceneManager.LoadScene("ContentGenerator");
            }
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.LevelSelectTheme));
        }

        private bool AllLevelsCompleted()
        {
            return LevelItems.All(level => level.Level.HasCompleted());
        }

        public void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            for (var i = 0; i < LevelItems.Count; ++i)
            {
                if (!LevelItems[i].IsSelected) continue;
                Selected.SelectLevel(i);
                SceneManager.LoadScene(/*"LevelWithEnemies"*/"Dungeon");
                return;
            }
        }
    }
    

}