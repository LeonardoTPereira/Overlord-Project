using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Game.GameManager.Player;
using Game.LevelManager.DungeonManager;


namespace Game.GameManager
{
    // TODO: Pula tela de level selection e carrega o nível gerado -> ao inves de carregar tela de level select,

    // TODO: 
    // Questão do loop -> Testar

    public class ExperimentController : MonoBehaviour
    {
        public static event EventHandler StartExperimentGeneratorEventHandler;
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        // [SerializeField, MustBeAssigned]
        // private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;

        private PlayerProfile selectedProfile;
        private List<QuestLineList> _questLinesListForProfile;

        public static bool UseFixedProfile => _useFixedProfile;
        private static bool _useFixedProfile;
        private static bool _updatedProfile = false;
        private static bool _firstRunCompleted = false;

        [SerializeField]
        private DungeonSceneLoader[] dungeonEntrances;

        private void Awake()
        {
            SetUseFixedProfile();
            _questLinesListForProfile = null;
        }

        private void OnEnable()
        {
            QuestGeneratorManager.FixedLevelProfileEventHandler += LoadDataForExperiment;
            QuestGeneratorManager.QuestLineCreatedEventHandler += SetQuestLinesForProfile;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

            PlayerController.PlayerDeathEventHandler += OnRunComplete;
            TriforceBhv.GotTriforceEventHandler += OnRunComplete;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.FixedLevelProfileEventHandler -= LoadDataForExperiment;
            QuestGeneratorManager.QuestLineCreatedEventHandler -= SetQuestLinesForProfile;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;

            PlayerController.PlayerDeathEventHandler -= OnRunComplete;
            TriforceBhv.GotTriforceEventHandler -= OnRunComplete;

            _questLinesListForProfile.Clear();
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "ContentGenerator" && _firstRunCompleted)
            {
                StartExperimentGeneratorEventHandler?.Invoke(null, EventArgs.Empty);
            }
            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }

        IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
        {
            yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
            SelectNarrativeAndSetDungeonsToEntrances();
        }

        private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld" && _questLinesListForProfile.Count > 0;
        }

        private void SelectNarrativeAndSetDungeonsToEntrances()
        {
            QuestLineList selectedQuestLine = GetAndRemoveRandomQuestLine();
            List<DungeonFileSo> dungeonFileSos = new List<DungeonFileSo>(selectedQuestLine.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonSceneLoader>();
            foreach (var dungeonEntrance in dungeonEntrances)
            {
                int selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrance.SelectedDungeon = dungeonFileSos[selectedIndex];
                dungeonEntrance.LevelQuestLines = selectedQuestLine;
                dungeonEntrance.IsLastQuestLine = _questLinesListForProfile.Count == 0;
                dungeonFileSos.RemoveAt(selectedIndex);
            }
        }

        private QuestLineList GetAndRemoveRandomQuestLine()
        {
            var selectedIndex = RandomSingleton.GetInstance().Random.Next(_questLinesListForProfile.Count);
            var questLines = _questLinesListForProfile[selectedIndex];
            _questLinesListForProfile.RemoveAt(selectedIndex);
            return questLines;
        }

        private void SetQuestLinesForProfile(object sender, QuestLineCreatedEventArgs eventArgs)
        {
            _questLinesListForProfile = new List<QuestLineList> { eventArgs.QuestLines };
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {
            selectedProfile = profileSelectedEventArgs.PlayerProfile;
            ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
        }

        private static void SetUseFixedProfile()
        {
            _useFixedProfile = RandomSingleton.GetInstance().Random.Next(0, 100) < 50;
        }

        private void OnRunComplete(object sender, EventArgs eventArgs)
        {
            _firstRunCompleted = true;
        }
    }
}