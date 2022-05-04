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
        private QuestLine _questLine;

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
            Debug.Log("Setting Quest Line In Dungeons: "+_questLine);
            Debug.Log(_questLine.DungeonFileSos.Count);
            Debug.Log(_questLine.EnemySos.Count);
            var dungeonFileSos = new List<DungeonFileSo>(_questLine.DungeonFileSos);
            dungeonEntrances = FindObjectsOfType<DungeonSceneLoader>();
            foreach (var dungeonEntrance in dungeonEntrances)
            {
                var selectedIndex = RandomSingleton.GetInstance().Random.Next(dungeonFileSos.Count);
                dungeonEntrance.SelectedDungeon = dungeonFileSos[selectedIndex];
                dungeonEntrance.LevelQuestLine = _questLine;
                dungeonEntrance.IsLastQuestLine = false;
                dungeonFileSos.RemoveAt(selectedIndex);
            }
        }
        
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForProfileToBeLoadedAndSelectNarratives(scene));
        }
        
        private bool CanLoadNarrativesToDungeonEntrances(Scene scene)
        {
            return scene.name == "Overworld";
        }
        
        private void LoadQuestData(object sender, QuestLineCreatedEventArgs questLineArgs)
        {
            _questLine = questLineArgs.Quests;
            Debug.Log("Quest Line Loaded: "+_questLine);
        }
    }
}