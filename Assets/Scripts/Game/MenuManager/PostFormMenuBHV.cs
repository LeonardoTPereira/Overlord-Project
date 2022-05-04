using Game.GameManager;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.MenuManager
{
    public class PostFormMenuBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField] private SceneReference levelSelector;
        
        public void GoToNext()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(levelSelector.SceneName);
        }

        public void GoToPrevious()
        {
            Debug.LogError("There is no Panel to Go Back To!");
        }
    }
}
