using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyComponents"), Serializable]
    public class EnemyComponentsSO : ScriptableObject
    {
        public MovementTypeRuntimeSetSO movementSet;
        public WeaponTypeRuntimeSetSO weaponSet;
        public BehaviorTypeRuntimeSetSO behaviorSet;
    }
}