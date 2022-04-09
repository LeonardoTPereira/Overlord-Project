using Game.Events;
using Game.NarrativeGenerator;
using Game.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class WeaponLoaderBHV : MonoBehaviour, IMenuPanel
    {
        ProjectileTypeSO projectileSO;
        [SerializeField]
        GameObject previousPanel;
        [SerializeField]
        Button button;
        [SerializeField]
        SceneReference levelToLoad;

        private bool isProjectileChosen = false;
        private bool isQuestGenerated = false;
        public static event LoadWeaponButtonEvent LoadWeaponButtonEventHandler;

        protected void OnEnable()
        {
            button.interactable = false;
            QuestGeneratorManager.QuestLineCreatedEventHandler += EnableNextButton;
            WeaponSelectionButtonBHV.SelectWeaponButtonEvent += PrepareWeapon;
        }

        protected void OnDisable()
        {
            WeaponSelectionButtonBHV.SelectWeaponButtonEvent -= PrepareWeapon;
            QuestGeneratorManager.QuestLineCreatedEventHandler -= EnableNextButton;
        }

        protected void PrepareWeapon(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            projectileSO = eventArgs.ProjectileSO;
            isProjectileChosen = true;
            if (isQuestGenerated)
            {
                button.interactable = true;
            }
        }
        
        private void EnableNextButton(object sender, QuestLineCreatedEventArgs args)
        {
            isQuestGenerated = true;
            if (isProjectileChosen)
            {
                button.interactable = true;
            }
        }

        public void GoToNext()
        {
            LoadWeaponButtonEventHandler?.Invoke(this, new LoadWeaponButtonEventArgs(projectileSO));
            SceneManager.LoadScene(levelToLoad.SceneName);
            gameObject.SetActive(false);
        }

        public void GoToPrevious()
        {
            previousPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
