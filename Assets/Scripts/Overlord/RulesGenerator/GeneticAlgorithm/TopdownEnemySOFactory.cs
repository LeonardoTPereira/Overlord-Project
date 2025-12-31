using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class TopdownEnemySOFactory: EnemySOFactory
    {
        public TopdownEnemySOFactory(EnemyMovementsSOInterface movementSet, EnemyWeaponsSOInterface weaponSet) : base(movementSet, weaponSet)
        {
        }

        public new List<EnemySO> GetEnemiesSOFromSolution(IEnumerable<Individual> solution)
        {
            var enemyList = new List<EnemySO>();

            foreach (var individual in solution)
            {
                int weaponIndex = Convert.ToInt32(individual.Weapon.Weapon);
                int movementIndex = Convert.ToInt32(individual.Enemy.Movement);
                ValidateIndices(weaponIndex, movementIndex);

                enemyList.Add(IndividualEnemySO(individual));
            }

            //ExportEnemiesToTextFile(enemyList, GetDocumentsFolderPath("EnemiesExport.txt"));    // DESATIVAR DEPOIS DE TESTES
            return enemyList;
        }

        protected new EnemySO IndividualEnemySO(Individual individual)
        {
            TopdownEnemySO enemySo = ScriptableObject.CreateInstance<TopdownEnemySO>();

            enemySo.Init(
                (int)individual.Enemy.Status1,
                (int)individual.Enemy.Status2,
                individual.Enemy.Status4,
                individual.Enemy.Status5,
                individual.Enemy.Status6,
                _weaponTypeSO.Items[Convert.ToInt32(individual.Weapon.Weapon)],
                _movementTypeSOList[Convert.ToInt32(individual.Enemy.Movement)],
                null, // Behavior not implemented yet
                individual.FitnessValue,
                individual.Enemy.Status3,
                individual.Weapon.WeaponStatus1
            );

            return enemySo;
        }
    }
}
