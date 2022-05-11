using System;
using Game.LevelSelection;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MenuManager
{
    public class GameOverPanelBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField] private GameObject nextPanel;
        [SerializeField] private SceneReference levelSelector;
        [SerializeField] private SceneReference levelWithEnemies;

        public static event EventHandler ToLevelSelectEventHandler;
        public static event EventHandler RestartLevelEventHandler;

        public LevelData currentLevel { private get; set; }
        
        public void GoToNext()
        {
            currentLevel.GiveUpLevel();
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void GoToSelector()
        {            
            ToLevelSelectEventHandler?.Invoke(null, EventArgs.Empty);
            gameObject.SetActive(false);
            SceneManager.LoadScene(levelSelector.SceneName);
        }

        public void GoToPrevious()
        {
            RestartLevelEventHandler?.Invoke(null, EventArgs.Empty);
            gameObject.SetActive(false);
            SceneManager.LoadScene(levelWithEnemies.SceneName);
        }
    }
}
