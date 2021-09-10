using EnemyGenerator;
using UnityEngine;

public class StaticSets
{
    public static MovementTypeRuntimeSetSO movementSet =
        Resources.Load<MovementTypeRuntimeSetSO>(
            "ScriptableObjectsData/MovementSet"
        );
    public static WeaponTypeRuntimeSetSO weaponSet =
        Resources.Load<WeaponTypeRuntimeSetSO>(
            "ScriptableObjectsData/WeaponSet"
        );
    public static BehaviorTypeRuntimeSetSO behaviorSet =
        Resources.Load<BehaviorTypeRuntimeSetSO>(
            "ScriptableObjectsData/BehaviorSet"
        );
}
