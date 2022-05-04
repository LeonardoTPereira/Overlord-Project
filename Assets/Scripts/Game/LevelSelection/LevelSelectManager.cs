using System.Collections.Generic;
using Game.Audio;
using Game.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class LevelSelectManager : MonoBehaviour, ISoundEmitter
    {
        [field: SerializeField] public SelectedLevels Selected { get; set; }
        [field: SerializeField] public List<LevelSelectItem> LevelItems { get; set; }

        public static event LevelLoadEvent LoadLevelEventHandler;
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
            ((ISoundEmitter)this).OnSoundEmitted(this, new PlayBgmEventArgs(AudioManager.BgmTracks.LevelSelectTheme));
        }

        public void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            for (var i = 0; i < LevelItems.Count; ++i)
            {
                if (!LevelItems[i].IsSelected) continue;
                Selected.SelectLevel(i);
                SceneManager.LoadScene("LevelWithEnemies");
                return;
            }
        }
    }
    

}