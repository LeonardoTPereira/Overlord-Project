using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class LevelSelectManager : MonoBehaviour
    {
        [field: SerializeField] public SelectedLevels Selected { get; set; }
        [field: SerializeField] public List<LevelSelectItem> LevelItems { get; set; }
        
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
            for (var i = 0; i < totalLevels; ++i)
            {
                LevelItems[i].levelData = Selected.Levels[i];
            }
        }
    }
    

}