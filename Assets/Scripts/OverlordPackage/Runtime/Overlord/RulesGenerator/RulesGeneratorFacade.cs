using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overlord.RulesGenerator;
using Overlord.RulesGenerator.EnemyGeneration;

namespace Overlord.GenerationController.Facade
{    
    public sealed class RulesGeneratorFacade
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
}
