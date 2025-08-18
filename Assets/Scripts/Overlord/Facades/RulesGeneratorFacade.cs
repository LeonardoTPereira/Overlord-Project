using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator;
using Overlord.RulesGenerator.EnemyGeneration;
using Game.EnemyGenerator;

namespace Overlord.GenerationController.Facade
{    
    public sealed class RulesGeneratorFacade//<IEnemy>
    {
        private static RulesGeneratorFacade _instance;
        public static RulesGeneratorFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RulesGeneratorFacade();
                }
                return _instance;
            }
        }

        private EnemyMovementsSOInterface _movementType;
        private SearchSpaceConfig _searchSpaceConfig;
        private EnemyGeneratorGeneticAlgorithmSettings _geneticSettings;

        public void SetEnemyMovementType(EnemyMovementsSOInterface movementType)
        {
            _movementType = movementType;
        }

        public EnemyMovementsSOInterface GetEnemyMovementType()
        {
            return _movementType;
        }

        public void SetGeneticSettings(EnemyGeneratorGeneticAlgorithmSettings geneticSettings)
        {
            _geneticSettings = geneticSettings;
        }

        public EnemyGeneratorGeneticAlgorithmSettings GetGeneticSettings()
        {
            return _geneticSettings;
        }

        public void SetSearchSpaceConfig(SearchSpaceConfig searchSpaceConfig)
        {
            _searchSpaceConfig = searchSpaceConfig;
        }

        public SearchSpaceConfig GetSearchSpaceConfig()
        {
            return _searchSpaceConfig;
        }

        /*
        public List<IEnemy> GetEnemies()
        {
            List<IEnemy> enemies = new List<IEnemy>();
            // Chama a função do gerador de inimigos e pega os inimigos
            Debug.Log("This is a log message from Enemy Facade.");
            return enemies;
        }
        */
    }

    public class TopdownEnemy : IEnemy
    {
        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        //[SerializeField]
        //public WeaponTypeSo weapon;
        //[SerializeField]
        //public MovementTypeSO movement;
        //[SerializeField]
        //public BehaviorTypeSO behavior;
        public float fitness;
        public float attackSpeed;
        public float projectileSpeed;
        
        
        
        // Transform in EnemySO
    }
}



