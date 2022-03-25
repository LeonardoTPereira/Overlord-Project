using System.Collections.Generic;
using System.Linq;
using Game.Events;
using Game.LevelGenerator;
using Game.Maestro;
using Game.NarrativeGenerator;
using MyBox;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    [RequireComponent(typeof(LevelGeneratorManager))]
    public class LevelGeneratorController : MonoBehaviour, IMenuPanel
    {
        public static event CreateEADungeonEvent createEADungeonEventHandler;
        private string playerProfile;

        protected Dictionary<string, TMP_InputField> inputFields;
        [SerializeField]
        protected GameObject progressCanvas, inputCanvas;
        protected TextMeshProUGUI progressTextUI;
        [SerializeField, Scene]
        protected string levelToLoad;
        protected string progressText;

        [Separator("Fitness Parameters to Create Dungeons")]
        [SerializeField]
        protected Fitness fitness;

        public void Awake()
        {
            if (inputCanvas != null)
            {
                inputCanvas.SetActive(true);
                inputFields = inputCanvas.GetComponentsInChildren<TMP_InputField>().ToDictionary(key => key.name, inputFieldObj => inputFieldObj);
            }

            if (progressCanvas == null) return;
            progressCanvas.SetActive(true);
            Debug.LogWarning(progressCanvas.transform.Find("ProgressPanel/ProgressText"));
            progressTextUI = progressCanvas.transform.Find("ProgressPanel/ProgressText").GetComponent<TextMeshProUGUI>();
            progressCanvas.SetActive(false);

        }

        public void OnEnable()
        {
            LevelGeneratorManager.newEAGenerationEventHandler += UpdateProgressBar;
            QuestGeneratorManager.ProfileSelectedEventHandler += CreateLevelFromNarrative;
        }
        public void OnDisable()
        {
            LevelGeneratorManager.newEAGenerationEventHandler -= UpdateProgressBar;
            QuestGeneratorManager.ProfileSelectedEventHandler -= CreateLevelFromNarrative;
        }

        public void CreateLevelFromNarrative(object sender, ProfileSelectedEventArgs eventArgs)
        {
            if (inputCanvas != null)
            {
                inputCanvas.SetActive(false);
            }

            if (progressCanvas != null)
            {
                progressCanvas.SetActive(true);
            }
        }

        public void CreateLevelFromInput()
        {
            int nRooms, nKeys, nLocks, nEnemies;
            float linearity;
            try
            {
                nRooms = int.Parse(inputFields["RoomsInputField"].text);
                nKeys = int.Parse(inputFields["KeysInputField"].text);
                nLocks = int.Parse(inputFields["LocksInputField"].text);
                nEnemies = int.Parse(inputFields["EnemiesInputField"].text);
                linearity = float.Parse(inputFields["LinearityInputField"].text);
                fitness = new Fitness(nRooms, nKeys, nLocks, nEnemies, linearity);
                createEADungeonEventHandler?.Invoke(this, new CreateEADungeonEventArgs(fitness));
            }
            catch (KeyNotFoundException)
            {
                Debug.LogWarning("Input Fields for Dungeon Generator incorrect. Using values from the Editor");
            }
            inputCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }

        public void UpdateProgressBar(object sender, NewEAGenerationEventArgs eventArgs)
        {
            progressText = eventArgs.CompletionRate.ToString() + "%";
            UnityMainThreadDispatcher.Instance().Enqueue(() => progressTextUI.text = progressText);
        }

        public void GoToNext()
        {
            SceneManager.LoadScene(levelToLoad);
        }

        public void GoToPrevious()
        {
            inputCanvas.SetActive(true);
            progressCanvas.SetActive(false);
        }
    }
}
