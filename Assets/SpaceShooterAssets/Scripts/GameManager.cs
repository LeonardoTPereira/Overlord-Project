using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.NarrativeGenerator;
using Overlord.LevelGenerator.Manager;
using Overlord.ProfileAnalyst;
using Overlord.RulesGenerator.EnemyGeneration;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyGeneratorManager _enemyGeneratorManager;
    [SerializeField] private LevelGeneratorManager _levelGeneratorManager;
    [SerializeField] private QuestGeneratorManager _questGeneratorManager;
    [SerializeField] private PlayerProfileManager _playerProfileManager;

    private void Start()
    {
        Debug.Log("GameManager started. Setting random player profile.");
        _playerProfileManager.SetPlayerProfileFromManualPlayerProfileSO();
        Debug.Log("Player profile set. Generating quests.");
    }


}
