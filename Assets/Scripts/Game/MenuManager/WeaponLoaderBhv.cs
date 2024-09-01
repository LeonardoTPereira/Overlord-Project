using Game.Events;
using Game.ExperimentControllers;
using Game.LevelSelection;
using Game.NarrativeGenerator;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace Game.MenuManager
{
    public class WeaponLoaderBhv : MonoBehaviour, IMenuPanel
    {
        [SerializeField] private GameObject previousPanel;
        [SerializeField] private Button button;
        [SerializeField] private SceneReference levelToLoad;
        [SerializeField] private GeneratorSettings settings;
        [field: SerializeField] public SelectedLevels Selected { get; set; }

        [field: SerializeField] public ProjectileTypeSO chosenProjectile { get; set; }

        private bool isProjectileChosen;
        private bool isQuestGenerated;

        protected void OnEnable()
        {
            button.interactable = false;
            QuestGeneratorManager.QuestLineCreatedEventHandler += EnableNextButton;
            WeaponSelectionButtonBhv.SelectWeaponButtonEvent += PrepareWeapon;
        }

        protected void OnDisable()
        {
            WeaponSelectionButtonBhv.SelectWeaponButtonEvent -= PrepareWeapon;
            QuestGeneratorManager.QuestLineCreatedEventHandler -= EnableNextButton;
        }

        private void PrepareWeapon(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            Debug.Log("Preparing Weapon");
            chosenProjectile.Copy(eventArgs.ProjectileSO);
            isProjectileChosen = true;
            if (isQuestGenerated)
            {
                button.interactable = true;
            }
        }
        
        private void EnableNextButton(object sender, QuestLineCreatedEventArgs args)
        {
            Debug.Log("Content Created and Button Enabled");
            isQuestGenerated = true;
            if (isProjectileChosen)
            {
                button.interactable = true;
            }
        }

        public void GoToNext()
        {
            if (Selected.selectedIndex == -1)
            {
                Selected.SelectLevel(null);
            }
            
            switch (settings.GameType)
            {
                case Enums.GameType.Platformer: SceneManager.LoadScene("Dungeon");
                    break;
                case Enums.GameType.TopDown: SceneManager.LoadScene("LevelWithEnemies");
                    break;
            }
            //SceneManager.LoadScene(levelToLoad.SceneName);
            gameObject.SetActive(false);
        }

        public void GoToPrevious()
        {
            previousPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
