using System.Collections;
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
    public class ExperimentController : MonoBehaviour
    {
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        [SerializeField, MustBeAssigned]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;
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
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler -= LoadDataForExperiment;
            QuestGeneratorManager.FixedLevelProfileEventHandler -= LoadDataForExperiment;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
        {
            yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
            SelectNarrativeAndSetDungeonsToEntrances();
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }

        private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld";
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

        private void SetQuestLinesForProfile(PlayerProfile playerProfile)
        {
            _questLinesListForProfile = new List<QuestLineList>(playerProfileToQuestLinesDictionarySo.QuestLinesForProfile[
                playerProfile.PlayerProfileEnum.ToString()]);
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {

            PlayerProfile selectedProfile;
            if (sender.GetType() == typeof(RealTimeLevelSelectManager))
            {
                selectedProfile = profileSelectedEventArgs.PlayerProfile;
                SetQuestLinesForProfile(selectedProfile);
            }
            else
            {
                if (RandomSingleton.GetInstance().Random.Next(0, 100) < 50)
                {
                    selectedProfile = profileSelectedEventArgs.PlayerProfile;
                }
                else
                {
                    selectedProfile = new PlayerProfile();
                    do
                    {
                        selectedProfile.PlayerProfileEnum = (PlayerProfile.PlayerProfileCategory)RandomSingleton.GetInstance().Random.Next(0, 4);
                    } while (selectedProfile.PlayerProfileEnum == profileSelectedEventArgs.PlayerProfile.PlayerProfileEnum);
                }
                ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
            }
        }
    }
}