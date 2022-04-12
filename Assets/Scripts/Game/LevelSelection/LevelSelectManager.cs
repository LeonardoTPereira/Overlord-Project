using System;
using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.GameManager.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class LevelSelectManager : MonoBehaviour
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
            foreach (var level in LevelItems)
            {
                if (!level.IsSelected) continue;
                LoadLevelEventHandler?.Invoke(this, new LevelLoadEventArgs(level.LevelData.Dungeon, level.LevelData.Quests, false));
                SceneManager.LoadScene("LevelWithEnemies");
                break;
            }
        }
    }
    

}