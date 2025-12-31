using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TopdownEnemySO", menuName = "Overlord-Project/Rules-Generator/TopdownEnemySO")]
    public class TopdownEnemySO : EnemySO
    {
        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        public float attackSpeed;
        public float projectileSpeed;

        public void Init(int _health, int _damage, float _movementSpeed, float _activeTime, float _restTime, WeaponTypeSo _weapon,
            MovementTypeSO _movement, BehaviorTypeSO _behavior, float _fitness, float _attackSpeed, float _projectileSpeed)
        {
            health = _health;
            damage = _damage;
            movementSpeed = _movementSpeed;
            activeTime = _activeTime;
            restTime = _restTime;
            weapon = _weapon;
            movement = _movement;
            behavior = _behavior;
            fitness = _fitness;
            attackSpeed = _attackSpeed;
            projectileSpeed = _projectileSpeed;
        }

        public static List<EnemySO> ConvertTopdownEnemySOListToEnemySOList(List<TopdownEnemySO> topdownEnemySOList)
        {
            List<EnemySO> enemySOList = new List<EnemySO>();
            foreach (var topdownEnemySO in topdownEnemySOList)
            {
                enemySOList.Add(convertTopdownEnemySOtoEnemySO(topdownEnemySO));
            }
            return enemySOList;
        }

        public static List<TopdownEnemySO> ConvertEnemySOListToTopdownEnemySOList(List<EnemySO> enemySOList)
        {
            List<TopdownEnemySO> topdownEnemySOList = new List<TopdownEnemySO>();
            foreach (var enemySO in enemySOList)
            {
                topdownEnemySOList.Add(convertEnemySOtoTopdownEnemySO(enemySO));
            }
            return topdownEnemySOList;
        }

        public static EnemySO convertTopdownEnemySOtoEnemySO(TopdownEnemySO topdownEnemySO)
        {
            EnemySO enemySO = ScriptableObject.CreateInstance<EnemySO>();
            enemySO.Init(
                topdownEnemySO.health,
                topdownEnemySO.damage,
                topdownEnemySO.movementSpeed,
                topdownEnemySO.activeTime,
                topdownEnemySO.restTime,
                topdownEnemySO.weapon,
                topdownEnemySO.movement,
                topdownEnemySO.behavior,
                topdownEnemySO.fitness,
                topdownEnemySO.attackSpeed,
                topdownEnemySO.projectileSpeed
            );
            return enemySO;
        }

        public static TopdownEnemySO convertEnemySOtoTopdownEnemySO(EnemySO enemySO)
        {
            TopdownEnemySO topdownEnemySO = ScriptableObject.CreateInstance<TopdownEnemySO>();
            topdownEnemySO.Init(
                (int)enemySO.status1,
                (int)enemySO.status2,
                enemySO.status3,
                enemySO.status4,
                enemySO.status5,
                enemySO.weapon,
                enemySO.movement,
                enemySO.behavior,
                enemySO.fitness,
                enemySO.status6,
                enemySO.weaponStatus1
            );
            return topdownEnemySO;
        }
    }
}