using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Audio;
using UnityEngine.InputSystem;
using MyBox;
using Game.LevelSelection;

namespace Game.MenuManager
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pausePressAction;
        [SerializeField] private GameObject _postFormQuestions;

        public static bool isGamePaused = false;
        public static float PausedTime;

        // Receive the PauseMenu game object
        public GameObject pauseMenuUI;
        public GameObject volumePanel;

        public TMP_Text menuLabelText;

        //private ScoreSystem scoreSystem;

        private AudioManager audioManager;

        public LevelData currentLevel { private get; set; }

        private void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            Resume();
            //scoreSystem = FindObjectOfType<ScoreSystem>();
            PausedTime = 0f;
        }

        private void Update()
        {
            if (_pausePressAction.action.triggered)
            {
                SwitchPaused();
            }
        }

        public void SwitchPaused()
        {
            // IF the game is already paused, exits the pause menu
            if (isGamePaused)
            {
                Resume();
            }
            // ELSE join the pause menu
            else
            {
                Pause();
            }
        }
        // Exit the Pause Menu
        public void Resume()
        {
            // Unfreeze the game
            Time.timeScale = 1f;
            //audioManager.PlaySFX("Button");
            // Exit the Pause Menu in the canvas
            pauseMenuUI.SetActive(false);


            // Set false meaning that the game is NOT paused
            isGamePaused = false;
        }

        // Open the Pause Menu
        public void Pause()
        {
            //audioManager.PlaySFX("Button");

            menuLabelText.text = "PAUSADO";
            // Show the Pause Menu in the canvas
            pauseMenuUI.SetActive(true);

            // Freeze the game
            Time.timeScale = 0f;

            // Set true meaning that the game is paused
            isGamePaused = true;

            //BackgroundImage.SetActive(true);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void BackToLevelSelection()
        {
            currentLevel.GiveUpLevel();
            _postFormQuestions.SetActive(true);
            gameObject.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Volume()
        {
            //audioManager.PlaySFX("Button");
            menuLabelText.text = "VOLUME";
            volumePanel.SetActive(true);
        }
    }
}