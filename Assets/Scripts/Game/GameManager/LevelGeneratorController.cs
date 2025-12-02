using Game.Events;
using Game.LevelGenerator;
using Game.MenuManager;
using Topdown.Overlord.NarrativeGenerator;
using MyBox;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Overlord.LevelGenerator.EvolutionaryAlgorithm;
using Overlord.Maestro.ExperimentControllers;

namespace Game.GameManager
{
    [RequireComponent(typeof(LevelGeneratorManager))]
    public class LevelGeneratorController : MonoBehaviour, IMenuPanel
    {
        public static event CreateEaDungeonEvent CreateEaDungeonEventHandler;
        private string playerProfile;

        private Dictionary<string, TMP_InputField> inputFields;
        [SerializeField]
        protected GameObject progressCanvas, inputCanvas;
        [SerializeField]
        private SceneReference levelToLoad;

        [Separator("Parameters to Create Dungeons")]
        [SerializeField]
        protected DungeonGeneratorGeneticAlgorithmSettings parameters;
        [SerializeField]
        protected FitnessInput fitnessInput;

        public void Awake()
        {
            if (inputCanvas != null)
            {
                inputCanvas.SetActive(true);
                inputFields = inputCanvas.GetComponentsInChildren<TMP_InputField>().ToDictionary(key => key.name, inputFieldObj => inputFieldObj);
            }

            if (progressCanvas == null) return;
            progressCanvas.SetActive(true);
            progressCanvas.SetActive(false);

        }

        public void OnEnable()
        {
            TopdownQuestGeneratorManager.FixedLevelProfileEventHandler += CreateLevelFromNarrative;
        }
        public void OnDisable()
        {
            TopdownQuestGeneratorManager.FixedLevelProfileEventHandler -= CreateLevelFromNarrative;
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
            try
            {
                var nRooms = int.Parse(inputFields["RoomsInputField"].text);
                var nKeys = int.Parse(inputFields["KeysInputField"].text);
                var nLocks = int.Parse(inputFields["LocksInputField"].text);
                var nEnemies = int.Parse(inputFields["EnemiesInputField"].text);
                var nitems = int.Parse(inputFields["ItemsInputField"].text);
                var nNpcs = int.Parse(inputFields["NpcsInputField"].text);
                var linearity = float.Parse(inputFields["LinearityInputField"].text);
                fitnessInput = new FitnessInput(nRooms, nKeys, nLocks, nEnemies, linearity, nitems, nNpcs, null, null);
                CreateEaDungeonEventHandler?.Invoke(this, new CreateEaDungeonEventArgs(parameters, fitnessInput,
                    false));
            }
            catch (KeyNotFoundException)
            {
                Debug.LogWarning("Input Fields for Dungeon Generator incorrect. Using values from the Editor");
            }
            inputCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }

        public void GoToNext()
        {
            SceneManager.LoadScene(levelToLoad.SceneName);
        }

        public void GoToPrevious()
        {
            inputCanvas.SetActive(true);
            progressCanvas.SetActive(false);
        }
    }
}
