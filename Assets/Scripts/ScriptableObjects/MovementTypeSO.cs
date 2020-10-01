using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGenerator
{
    public delegate Vector3 MovementType(Vector3 playerPos, Vector3 enemyPos);

    [CreateAssetMenu]
    public class MovementTypeSO : ScriptableObject
    {
        
        public float multiplier;
        public MovementEnum enemyMovementIndex;
        public MovementType movementType;
    }
}
