using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;
using ScriptableObjects;

public class SpaceShooterEnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyGeneratorManager _enemyGeneratorManager;
    public List<EnemySO> Enemies;

    void Start()
    {
        Enemies = _enemyGeneratorManager.GetEnemySOList(DifficultyLevels.Hard);
        Debug.Log("Start Ended");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
