using System;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    public delegate Vector2 MovementType(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask);

    [CreateAssetMenu(fileName = "MovementTypeSO", menuName = "Overlord-Project/Rules-Generator/Util/MovementType")]
    public class MovementTypeSO : ScriptableObject
    {
        public float multiplier;
        public Enums.MovementEnum enemyMovementIndex;
        [NonSerialized] public MovementType movementType;
    }
}