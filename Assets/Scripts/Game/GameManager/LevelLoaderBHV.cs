using Game.Events;
using Game.MenuManager;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class LevelLoaderBHV : MonoBehaviour, IMenuPanel
    {
        string levelFile;
        [SerializeField]
        GameObject previousPanel, nextPanel;
        [SerializeField]
        Button button;
        public static event LevelLoadEvent loadLevelButtonEventHandler;

        protected void OnEnable()
        {
            button.interactable = false;
            LevelSelectButtonBhv.SelectLevelButtonEventHandler += PrepareLevel;
        }

        protected void OnDisable()
        {
            LevelSelectButtonBhv.SelectLevelButtonEventHandler -= PrepareLevel;
        }

        protected void PrepareLevel(object sender, LevelSelectEventArgs args)
        {
            levelFile = args.LevelSO.fileName;
            button.interactable = true;
        }

        public void GoToNext()
        {
            loadLevelButtonEventHandler?.Invoke(this, new LevelLoadEventArgs(levelFile));
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void GoToPrevious()
        {
            previousPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
