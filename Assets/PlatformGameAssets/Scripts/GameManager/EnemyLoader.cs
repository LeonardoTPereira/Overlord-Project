using System.Collections;
using System.Collections.Generic;
using PlatformGame.Enemy;
using ScriptableObjects;
using UnityEngine;

namespace PlatformGame.GameManager
{
    public class EnemyLoader : MonoBehaviour
    {
        [SerializeField] private GameObject enemyCopy;
        [SerializeField] private EnemySO[] enemySos;
        void Start()
        {
            foreach (var enemySo in enemySos)
            {
                GameObject enemy = Instantiate(enemyCopy);
                enemySo.movement.movementType = FollowPlayer;
                enemy.GetComponent<EnemyController>().LoadEnemy(enemySo);
            }
        }

        private Vector2 FollowPlayer(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            Vector2 direction = playerPos - enemyPos;
            return direction.normalized;
        }
    }
}