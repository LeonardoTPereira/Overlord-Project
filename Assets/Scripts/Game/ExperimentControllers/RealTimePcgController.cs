using System.Collections;
using System.Collections.Generic;
using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.ExperimentControllers
{
    public class RealTimePcgController : MonoBehaviour
    {
        private QuestLineList _questLines;

        [SerializeField]
        private DungeonSceneLoader[] dungeonEntrances;
        
        private void OnEnable()
        {
            QuestGeneratorManager.QuestLineCreatedEventHandler += LoadQuestData;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            QuestGeneratorManager.QuestLineCreatedEventHandler -= LoadQuestData;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private IEnumerator WaitForProfileToBeLoadedAndSelectNarratives(Scene scene)
        {
            yield return new WaitUntil(() => CanLoadNarrativesToDungeonEntrances(scene));
            SetQuestLineInDungeons();
        }
        
        private void SetQuestLineInDungeons()
        {
            var dungeonFileSos = new List<DungeonFileSo>(_questLines.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonSceneLoader>();
            foreach (var dungeonEntrance in dungeonEntrances)
            {
                var selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrance.SelectedDungeon = dungeonFileSos[selectedIndex];
                dungeonEntrance.LevelQuestLines = _questLines;
                dungeonEntrance.IsLastQuestLine = false;
                dungeonFileSos.RemoveAt(selectedIndex);
            }
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }
        
        private static bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld";
        }
        
        private void LoadQuestData(object sender, QuestLineCreatedEventArgs questLineArgs)
        {
            _questLines = questLineArgs.QuestLines;
            Debug.Log("Quest Line Loaded: "+_questLines);
        }
    }
}