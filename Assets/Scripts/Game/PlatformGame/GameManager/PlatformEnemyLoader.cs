using System.Collections;
using System.Collections.Generic;
using Game.GameManager;
using ScriptableObjects;
using UnityEngine;
using EnemyController = PlatformGame.Enemy.EnemyController;

namespace PlatformGame.GameManager
{
    public class PlatformEnemyLoader : EnemyLoader
    {
        public override GameObject InstantiateEnemyFromScriptableObject(Vector3 position, Quaternion rotation, EnemySO enemySo, int questId)
        {
            GameObject enemy;
            //TODO change to use weaponType in comparison
            if (enemySo.weapon.name == "None")
            {
                enemy = Instantiate(BareHandEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Bow")
            {
                enemy = Instantiate(ShooterEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "BombThrower")
            {
                enemy = Instantiate(BomberEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Cure")
            {
                enemy = Instantiate(HealerEnemyPrefab, position, rotation);
            }
            else
            {
                enemy = Instantiate(EnemyPrefab, position, rotation);
            }

            enemy.GetComponent<EnemyController>().LoadEnemyData(enemySo, questId);
            return enemy;
        }

    }
}