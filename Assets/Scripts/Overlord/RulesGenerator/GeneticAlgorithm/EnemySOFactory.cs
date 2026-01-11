using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public class EnemySOFactory : MonoBehaviour
    {
        protected EnemyMovementsSOInterface _movementSet;
        protected EnemyWeaponsSOInterface _weaponSet;
        // TODO: Remover totalmente MovementTypeSO e utilizar apenas EnemyMovementsSOInterface _movementSet
        protected List<MovementTypeSO> _movementTypeSOList;
        protected WeaponTypeRuntimeSetSO _weaponTypeSO;

        public EnemySOFactory(EnemyMovementsSOInterface movementSet, EnemyWeaponsSOInterface weaponSet)
        {
            _movementSet = movementSet ?? throw new ArgumentNullException(nameof(movementSet));
            _weaponSet = weaponSet ?? throw new ArgumentNullException(nameof(weaponSet));

            _movementTypeSOList = OverlordDataToGameDataConverter.ToMovementTypeSOList(_movementSet);
            _weaponTypeSO = FindObjectOfType<OverlordDataToGameDataConverter>().WeaponSet;
        }

        public List<EnemySO> GetEnemiesSOFromSolution(IEnumerable<Individual> solution)
        {
            var enemyList = new List<EnemySO>();

            foreach (var individual in solution)
            {
                ValidateIndices(_weaponSet.GetMappedIndex(individual.Weapon.Weapon), 
                    _movementSet.GetMappedIndex(individual.Enemy.Movement));

                enemyList.Add(IndividualEnemySO(individual));
            }
            //ExportEnemiesToTextFile(enemyList, GetDocumentsFolderPath("EnemiesExport.txt"));    // DESATIVAR DEPOIS DE TESTES
            return enemyList;
        }

        protected void ValidateIndices(int weaponIndex, int movementIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= _weaponSet.GetAllWeaponTypes().Count)
            {
                throw new IndexOutOfRangeException($"Weapon index {weaponIndex} is out of range.");
            }
            if (movementIndex < 0 || movementIndex >= _movementSet.GetEnemyMovementCount())
            {
                throw new IndexOutOfRangeException($"Movement index {movementIndex} is out of range.");
            }
        }

        protected EnemySO IndividualEnemySO(Individual individual)
        {
            EnemySO enemySo = ScriptableObject.CreateInstance<EnemySO>();
            enemySo.Init(
                individual.Enemy.Status1,
                individual.Enemy.Status2,
                individual.Enemy.Status3,
                individual.Enemy.Status4,
                individual.Enemy.Status5,
                _weaponTypeSO.Items[Convert.ToInt32(individual.Weapon.Weapon)],
                _movementTypeSOList[Convert.ToInt32(individual.Enemy.Movement)],
                null, // Behavior not implemented yet
                individual.FitnessValue,
                individual.Enemy.Status6,
                individual.Weapon.WeaponStatus1
            );
            return enemySo;
        }

        public void ExportEnemiesToTextFile(List<TopdownEnemySO> enemies, string path)
        {
            if (enemies == null || enemies.Count == 0)
            {
                UnityEngine.Debug.LogWarning("[EnemySOFactory] No enemies to export.");
                return;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < enemies.Count; i++)
            {
                TopdownEnemySO enemy = enemies[i];

                sb.AppendLine($"Enemy#{i + 1:000}:");
                sb.AppendLine($"  Health: {enemy.health}");
                sb.AppendLine($"  Strength: {enemy.damage}");
                sb.AppendLine($"  Movement Speed: {enemy.movementSpeed}");
                sb.AppendLine($"  Active Time: {enemy.activeTime}");
                sb.AppendLine($"  Rest Time: {enemy.restTime}");
                sb.AppendLine($"  Weapon: {(enemy.weapon != null ? enemy.weapon.EnemyTypeName : "NULL")}");
                sb.AppendLine($"  Movement: {(enemy.movement != null ? enemy.movement.enemyMovementIndex.ToString() : "NULL")}");
                sb.AppendLine($"  Fitness: {enemy.fitness}");
                sb.AppendLine($"  Attack Speed: {enemy.attackSpeed}");
                sb.AppendLine($"  Projectile Speed: {enemy.projectileSpeed}");
                sb.AppendLine(); // empty line between enemies
            }

            try
            {
                File.WriteAllText(path, sb.ToString());
                UnityEngine.Debug.Log($"[EnemySOFactory] Exported {enemies.Count} enemies to {path}");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[EnemySOFactory] Failed to write file at {path}. Exception: {ex.Message}");
            }
        }

        protected string GetDocumentsFolderPath(string fileName)
        {
            // Path to the Documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Create subfolder (optional, to organize better)
            string exportFolder = Path.Combine(documentsPath, "Overlord-Project_Exports");
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            // Final path
            return Path.Combine(exportFolder, fileName);
        }
    }
}
