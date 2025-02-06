using System.Collections;
using System;
using System.Collections.Generic;
using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using Game.LevelSelection;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

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
        private List<QuestLineList> _questLinesListForProfile;

        [SerializeField]
        private DungeonSceneLoader[] dungeonEntrances;

        private void Awake()
        {
            _questLinesListForProfile = null;
        }

        private void OnEnable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler += LoadDataForExperiment;
            QuestGeneratorManager.FixedLevelProfileEventHandler += LoadDataForExperiment;
            QuestGeneratorManager.QuestLineCreatedEventHandler += SetQuestLinesForProfile;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler -= LoadDataForExperiment;
            QuestGeneratorManager.FixedLevelProfileEventHandler -= LoadDataForExperiment;
            QuestGeneratorManager.QuestLineCreatedEventHandler -= SetQuestLinesForProfile;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
        {
            yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
            SelectNarrativeAndSetDungeonsToEntrances();
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "ContentGenerator")
            {
                StartExperimentGeneratorEventHandler?.Invoke(null, EventArgs.Empty);
            }

            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }

        private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld";
        }

        private void SelectNarrativeAndSetDungeonsToEntrances()
        {
            Debug.Log("select narratives and dungeon entrances");
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
            Debug.Log("set questlines for profile");
            _questLinesListForProfile = new List<QuestLineList> {eventArgs.QuestLines} ;
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {

            PlayerProfile selectedProfile;

            // if (sender.GetType() != typeof(RealTimeLevelSelectManager))
            {
                if ( !UseTrueProfile() )
                {
                    profileSelectedEventArgs.PlayerProfile.SetAsComplementaryProfile(); 
                }
            }
            selectedProfile = profileSelectedEventArgs.PlayerProfile;

            // SetQuestLinesForProfile(selectedProfile);
            ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
            Debug.Log("select experiment profile");
        }

        private static bool UseTrueProfile()
        {
            return RandomSingleton.GetInstance().Random.Next(0, 100) < 50;
        }
    }
}