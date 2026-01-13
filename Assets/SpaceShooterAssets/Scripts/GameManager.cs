using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.NarrativeGenerator;
using Overlord.LevelGenerator.Manager;
using Overlord.ProfileAnalyst;
using Overlord.RulesGenerator.EnemyGeneration;
using Overlord.GenerationController;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyGeneratorManager _enemyGeneratorManager;
    [SerializeField] private LevelGeneratorManager _levelGeneratorManager;
    [SerializeField] private QuestGeneratorManager _questGeneratorManager;
    [SerializeField] private PlayerProfileManager _playerProfileManager;

    [SerializeField] private EnemyLoader _enemyLoader;

    private void Start()
    {
        Debug.Log("GameManager started. Setting random player profile.");
        _playerProfileManager.SetPlayerProfileFromManualPlayerProfileSO();

        StartCoroutine(WaitForDungeonGeneration());
    }

    private IEnumerator WaitForDungeonGeneration()
    {
        while (!GenerationStatus.EndedDungeonGeneration)
        {
            Debug.Log("Waiting for dungeon generation to end...");
            yield return null; // espera 1 frame
        }

        Debug.Log("Dungeon generation ended. Loading level and enemies.");
        LevelLoader.Instance.Load(
            _questGeneratorManager.questLines,
            _questGeneratorManager.questLines.DungeonFileSos[0]
        );
        Debug.Log("Loading minimap.");
        MinimapController.Instance.Build(_questGeneratorManager.questLines.DungeonFileSos[0]);

    }
}
