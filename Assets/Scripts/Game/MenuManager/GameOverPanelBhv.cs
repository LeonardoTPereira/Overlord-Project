using System;
using Game.LevelSelection;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.MenuManager
{
    public class GameOverPanelBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField] private GameObject nextPanel;
        [SerializeField] private SceneReference levelSelector;
        [SerializeField] private SceneReference levelWithEnemies;

        public LevelData currentLevel { private get; set; }
        
        public void GoToNext()
        {
            currentLevel.GiveUpLevel();
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void GoToSelector()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(levelSelector.SceneName);
        }

        public void GoToPrevious()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(levelWithEnemies.SceneName);
        }
    }
}
