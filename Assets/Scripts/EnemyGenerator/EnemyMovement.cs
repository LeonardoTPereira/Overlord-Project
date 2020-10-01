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
    }
}