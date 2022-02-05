using System.Collections.Generic;
using System.Linq;
using Game.EnemyManager;
using Game.Maestro;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using static Util.Enums;

namespace Game.GameManager
{
    public class EnemyLoader : MonoBehaviour
    {
        private List<EnemySO> enemyListForCurrentDungeon;

    
        public WeaponTypeRuntimeSetSO WeaponTypes => _weaponTypes;

        [SerializeField]
        public EnemySO[] arena;
        public GameObject enemyPrefab;
        public GameObject barehandEnemyPrefab;
        public GameObject shooterEnemyPrefab;
        public GameObject bomberEnemyPrefab;
        public GameObject healerEnemyPrefab;
        [SerializeField]
        [MustBeAssigned] private WeaponTypeRuntimeSetSO _weaponTypes;

        public void LoadEnemies(List<EnemySO> enemyList)
        {
            enemyListForCurrentDungeon = EnemySelector.FilterEnemies(enemyList);
            ApplyDelegates();
        }

        public EnemySO GetRandomEnemyOfType(WeaponTypeSO enemyType)
        {
            List<EnemySO> currentEnemies = GetEnemiesFromType(enemyType);
            return currentEnemies[Random.Range(0, currentEnemies.Count)];
        }

        public GameObject InstantiateEnemyWithType(Vector3 position, Quaternion rotation, WeaponTypeSO enemyType)
        {
            EnemySO currentEnemy = GetRandomEnemyOfType(enemyType);
            GameObject enemy;
            //TODO change to use weaponType in comparison
            if (currentEnemy.weapon.name == "None")
            {
                enemy = Instantiate(barehandEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "Bow")
            {
                enemy = Instantiate(shooterEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "BombThrower")
            {
                enemy = Instantiate(bomberEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "Cure")
            {
                enemy = Instantiate(healerEnemyPrefab, position, rotation);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, rotation);
            }
            enemy.GetComponent<EnemyController>().LoadEnemyData(currentEnemy);
            return enemy;
        }
    
        public GameObject InstantiateEnemyFromScriptableObject(Vector3 position, Quaternion rotation, EnemySO enemySo)
        {
            GameObject enemy;
            //TODO change to use weaponType in comparison
            if (enemySo.weapon.name == "None")
            {
                enemy = Instantiate(barehandEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Bow")
            {
                enemy = Instantiate(shooterEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "BombThrower")
            {
                enemy = Instantiate(bomberEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Cure")
            {
                enemy = Instantiate(healerEnemyPrefab, position, rotation);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, rotation);
            }
            enemy.GetComponent<EnemyController>().LoadEnemyData(enemySo);
            return enemy;
        }

        private List<EnemySO> GetEnemiesFromType(WeaponTypeSO weaponType)
        {
            //TODO create these lists only once per type on dungeon load
            return enemyListForCurrentDungeon.Where(enemy => enemy.weapon == weaponType).ToList();
        }

        private void ApplyDelegates()
        {
            foreach (var enemy in enemyListForCurrentDungeon)
            {
                enemy.movement.movementType = GetMovementType(enemy.movement.enemyMovementIndex);
            }
        }
        public MovementType GetMovementType(MovementEnum moveTypeEnum)
        {
            switch (moveTypeEnum)
            {
                case MovementEnum.None:
                    return EnemyMovement.NoMovement;
                case MovementEnum.Random:
                    return EnemyMovement.MoveRandomly;
                case MovementEnum.Flee:
                    return EnemyMovement.FleeFromPlayer;
                case MovementEnum.Follow:
                    return EnemyMovement.FollowPlayer;
                case MovementEnum.Follow1D:
                    return EnemyMovement.FollowPlayer1D;
                case MovementEnum.Random1D:
                    return EnemyMovement.MoveRandomly1D;
                case MovementEnum.Flee1D:
                    return EnemyMovement.FleeFromPlayer1D;
                default:
                    Debug.Log("No Movement Attached to Enemy");
                    return null;
            }
        }
    }
}
