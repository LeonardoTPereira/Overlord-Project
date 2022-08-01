using UnityEngine;

namespace Game.EnemyManager
{
    public class EnemyMovement : MonoBehaviour
    {
        public static Vector2 MoveRandomly(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));
        }

        public static Vector2 NoMovement(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return new Vector2(0, 0);
        }

        public static Vector2 FollowPlayer(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return playerPos - enemyPos;
        }

        public static Vector2 FleeFromPlayer(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return enemyPos - playerPos;
        }

        public static Vector2 FollowPlayer1D(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            var movementDir = playerPos - enemyPos;
            return GetMovementIn1D(ref directionMask, updateMask, movementDir);
        }

        public static Vector2 FleeFromPlayer1D(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            var movementDir = enemyPos - playerPos;
            return GetMovementIn1D(ref directionMask, updateMask, movementDir);
        }

        public static Vector2 MoveRandomly1D(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            var movementDir = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));
            return GetMovementIn1D(ref directionMask, updateMask, movementDir);
        }

        private static Vector2 GetMovementIn1D(ref Vector2 directionMask, bool updateMask, Vector2 movementDir)
        {
            if (!updateMask) return movementDir * directionMask;
            directionMask = Random.value < 0.5f ? new Vector2(0, 1) : new Vector2(1, 0);
            return movementDir * directionMask;
        }
    }
}