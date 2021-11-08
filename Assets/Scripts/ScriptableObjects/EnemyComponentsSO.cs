using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyComponents")]
    public class EnemyComponentsSO : ScriptableObject
    {
        public MovementTypeRuntimeSetSO movementSet;
        public WeaponTypeRuntimeSetSO weaponSet;
        public BehaviorTypeRuntimeSetSO behaviorSet;
    }
}