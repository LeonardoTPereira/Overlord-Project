using Game.Events;
using Game.MenuManager;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class LevelLoaderBHV : MonoBehaviour, IMenuPanel
    {
        [SerializeField]
        GameObject previousPanel, nextPanel;
        [SerializeField]
        Button button;

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
            button.interactable = true;
        }

        public void GoToNext()
        {
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
