using UnityEngine;

namespace ScriptableObjects
{
    public delegate Vector3 BehaviorType(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos);
    [CreateAssetMenu]
    public class BehaviorTypeSO : ScriptableObject
    {
        public float multiplier;
        public BehaviorType enemyBehavior;
    }
}
