using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MenuManager
{
    public class MenuTransitionPanelBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField]
        protected GameObject previousPanel;
        [SerializeField]
        protected SceneReference nextPanel;
        public void GoToNext()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(nextPanel.SceneName);
        }
        public void GoToPrevious()
        {
            previousPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
