using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGenerator
{
    public class EnemyMovement : MonoBehaviour
    {
        public static Vector3 MoveRandomly(Vector3 playerPos, Vector3 enemyPos)
        {
            return new Vector3(Random.Range(-10, 11), Random.Range(-10, 11), 0);
        }
        public static Vector3 NoMovement(Vector3 playerPos, Vector3 enemyPos)
        {
            return new Vector3(0, 0, 0);
        }
        public static Vector3 FollowPlayer(Vector3 playerPos, Vector3 enemyPos)
        {
            return playerPos - enemyPos;
        }
        public static Vector3 FleeFromPlayer(Vector3 playerPos, Vector3 enemyPos)
        {
            return enemyPos - playerPos;
        }

        public static Vector3 FollowPlayer1D(Vector3 playerPos, Vector3 enemyPos)
        {
            Vector3 movementDir = playerPos - enemyPos;
            if (Random.value < 0.5f)
                movementDir.x = 0;
            else
                movementDir.y = 0;
            return movementDir;
        }
        public static Vector3 FleeFromPlayer1D(Vector3 playerPos, Vector3 enemyPos)
        {
            Vector3 movementDir = enemyPos - playerPos;
            if (Random.value < 0.5f)
                movementDir.x = 0;
            else
                movementDir.y = 0;
            return movementDir;
        }

        public static Vector3 MoveRandomly1D(Vector3 playerPos, Vector3 enemyPos)
        {
            Vector3 movementDir = new Vector3(Random.Range(-10, 11), Random.Range(-10, 11), 0);
            if (Random.value < 0.5f)
                movementDir.x = 0;
            else
                movementDir.y = 0;
            return movementDir;
        }
    }
}