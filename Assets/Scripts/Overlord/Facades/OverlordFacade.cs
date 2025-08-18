using System.Collections;
using System;
using UnityEngine;
using Overlord.RulesGenerator.EnemyGeneration;

namespace Overlord.GenerationController.Facade
{
    public sealed class OverlordFacade
    {
        private static OverlordFacade _instance;
        public static OverlordFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OverlordFacade();
                }
                return _instance;
            }
        }

        private RulesGeneratorFacade _rulesGeneratorFacade = RulesGeneratorFacade.Instance;
        //private NarrativeGeneratorFacade _narrativeGeneratorFacade = NarrativeGeneratorFacade.Instance;
        // private LevelsGeneratorFacade _levelsGeneratorFacade = LevelsGeneratorFacade.Instance;
        /*
        public void SetEnemyMovements(EnemyMovementsSO<Enum> movementType)
        {
            //var movementType = new TopdownMovementType();
            _rulesGeneratorFacade.SetEnemyMovementType(movementType);
        }

        public void SetEnemyWeapons()
        {
            // _rulesGeneratorFacade.SetEnemyWeaponType(weaponType);
        }

        */
    }
}
