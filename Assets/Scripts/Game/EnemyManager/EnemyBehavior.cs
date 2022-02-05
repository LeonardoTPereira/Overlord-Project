using UnityEngine;

namespace Game.EnemyManager
{
    public class EnemyBehavior : MonoBehaviour
    {
        public static Vector3 IndifferentBehavior(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos)
        {
            //Just move as the movement funtion says.
            return new Vector3(0, 0, 0);
        }
        public static Vector3 LoneWolfBehavior(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos)
        {
            //Process other enemies position and the resulting vector must point away from the average position of every one
            Vector3 resultingDir = new Vector3(0, 0, 0);
            foreach (Vector3 otherPos in otherEnemiesPos)
            {
                resultingDir = resultingDir + enemyPos - otherPos;
            }
            return resultingDir;
        }

        public static Vector3 SwarmBehavior(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos)
        {
            //Process other enemies position and the resulting vector must point the average position of every one
            Vector3 resultingDir = new Vector3(0, 0, 0);
            foreach (Vector3 otherPos in otherEnemiesPos)
            {
                resultingDir = resultingDir + otherPos - enemyPos;
            }
            return resultingDir;
        }

        //WARNING - THIS ONE IS INCOMPLETE
        public static Vector3 PincerBehavior(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos)
        {
            //Process other enemies position and the resulting vector must point to the average position of the least enemies away from the player
            Vector3 resultingDir = new Vector3(0, 0, 0);
            foreach (Vector3 otherPos in otherEnemiesPos)
            {
                //TODO: Make calculations that i have no idea what will be
                resultingDir = resultingDir + otherPos - enemyPos;
            }
            return resultingDir;
        }
    }
}
