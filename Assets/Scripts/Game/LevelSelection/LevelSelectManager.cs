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

        [field: SerializeField] private LevelData _levelToLoad;
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
            LoadLevelsToItems();
        }

        private void LoadLevelsToItems()
        {
            Debug.Log("Loading levels to items");
            var totalLevels = LevelItems.Count;
            for (var i = 0; i < totalLevels; i++)
            {
                LevelItems[i].LevelData = Selected.Levels[i];
            }
        }
        
        public void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            foreach (var level in LevelItems)
            {
                if (!level.IsSelected) continue;
                _levelToLoad.Dungeon = level.LevelData.Dungeon;
                _levelToLoad.Quests = level.LevelData.Quests;
                SceneManager.LoadScene("LevelWithEnemies");
                return;
            }
        }
    }
    

}