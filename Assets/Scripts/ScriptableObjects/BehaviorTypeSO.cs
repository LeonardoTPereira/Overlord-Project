using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGenerator
{
    public delegate Vector3 BehaviorType(Vector3 playerPos, Vector3 enemyPos, Vector3[] otherEnemiesPos);
    [CreateAssetMenu]
    public class BehaviorTypeSO : ScriptableObject
    {
        public float multiplier;
        public BehaviorType enemyBehavior;
    }
}
