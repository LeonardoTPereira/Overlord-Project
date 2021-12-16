using System;
using System.Collections;
using System.Collections.Generic;
using Game.Maestro;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Random = UnityEngine.Random;

namespace Game.GameManager
{
    public class ExperimentController : MonoBehaviour
    {
        public static event ProfileSelectedEvent ProfileSelectedEventHandler;

        [SerializeField, MustBeAssigned]
        private PlayerProfileToQuestLinesDictionarySo playerProfileToQuestLinesDictionarySo;
        private List<QuestLine> _questLineListForProfile;

        [SerializeField]
        private DungeonLoader[] dungeonEntrances;

        private void Awake()
        {
            _questLineListForProfile = null;
        }

        private void OnEnable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler += LoadDataForExperiment;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.ProfileSelectedEventHandler -= LoadDataForExperiment;
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
            QuestLine selectedQuestLine = GetAndRemoveRandomQuestLine();
            List<DungeonFileSo> dungeonFileSos = new List<DungeonFileSo>(selectedQuestLine.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonLoader>();
            foreach (var dungeonEntrance in dungeonEntrances)
            {
                int selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrance.SelectedDungeon = dungeonFileSos[selectedIndex];
                dungeonEntrance.LevelQuestLine = selectedQuestLine;
                dungeonEntrance.IsLastQuestLine = _questLineListForProfile.Count == 0;
                dungeonFileSos.RemoveAt(selectedIndex);
            }
        }

        private QuestLine GetAndRemoveRandomQuestLine()
        {
            QuestLine questLine;
            int selectedIndex = RandomSingleton.GetInstance().Random.Next(_questLineListForProfile.Count);
            questLine = _questLineListForProfile[selectedIndex];
            _questLineListForProfile.RemoveAt(selectedIndex);
            return questLine;
        }

        private void SetQuestLinesForProfile(PlayerProfile playerProfile)
        {
            _questLineListForProfile = new List<QuestLine>(playerProfileToQuestLinesDictionarySo.QuestLinesForProfile[
                playerProfile.PlayerProfileEnum.ToString()].QuestLinesList);
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {
            PlayerProfile selectedProfile;
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
            SetQuestLinesForProfile(selectedProfile);
        }
    }
}