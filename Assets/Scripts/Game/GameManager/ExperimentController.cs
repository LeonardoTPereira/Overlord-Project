using System.Collections;
using System.Collections.Generic;
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
        private PlayerProfileToQuestLinesDictionarySO playerProfileToQuestLinesDictionarySo;
        private List<QuestLine> _questLineListForProfile;

        [SerializeField]
        private DungeonEntrance[] dungeonEntrances;

        private void OnEnable()
        {
            Manager.ProfileSelectedEventHandler += LoadDataForExperiment;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            Manager.ProfileSelectedEventHandler -= LoadDataForExperiment;
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
            List<DungeonFileSO> dungeonFileSos = new List<DungeonFileSO>(selectedQuestLine.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonEntrance>();
            for (int i = 0; i < dungeonEntrances.Length; ++i)
            {
                int selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrances[i].DungeonFileSo = dungeonFileSos[selectedIndex];
                dungeonFileSos.RemoveAt(selectedIndex);
                Debug.Log("Dungeon Entrance Filename: " + dungeonEntrances[i].DungeonFileSo.name);
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
                playerProfile.PlayerProfileEnum.ToString()].QuestLines);
        }

        private void LoadDataForExperiment(object sender, ProfileSelectedEventArgs profileSelectedEventArgs)
        {
            Debug.Log("Loading Data For Experiment. Profile: " + profileSelectedEventArgs.PlayerProfile.ToString());
            PlayerProfile selectedProfile;
            if (Random.Range(0, 100) < 50)
            {
                selectedProfile = profileSelectedEventArgs.PlayerProfile;
            }
            else
            {
                selectedProfile = new PlayerProfile();
                do
                {
                    selectedProfile.PlayerProfileEnum = (PlayerProfile.PlayerProfileCategory)Random.Range(0, 4);
                } while (selectedProfile.PlayerProfileEnum == profileSelectedEventArgs.PlayerProfile.PlayerProfileEnum);
            }
            ProfileSelectedEventHandler?.Invoke(null, new ProfileSelectedEventArgs(selectedProfile));
            SetQuestLinesForProfile(selectedProfile);
        }
    }
}