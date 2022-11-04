using UnityEngine;

namespace Game.MenuManager
{
    public class MenuPanelBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField]
        protected GameObject previousPanel, nextPanel;

        public void GoToNext()
        {
            Debug.Log("Going to Next Panel");
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
