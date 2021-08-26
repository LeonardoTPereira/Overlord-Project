using EnemyGenerator;
using UnityEditor;

public class StaticSets
{
    public static MovementTypeRuntimeSetSO movementSet =
        AssetDatabase.LoadAssetAtPath<MovementTypeRuntimeSetSO>(
            "Assets/ScriptableObjectsData/MovementSet.asset"
        );
    public static WeaponTypeRuntimeSetSO weaponSet =
        AssetDatabase.LoadAssetAtPath<WeaponTypeRuntimeSetSO>(
            "Assets/ScriptableObjectsData/WeaponSet.asset"
        );
    public static BehaviorTypeRuntimeSetSO behaviorSet =
        AssetDatabase.LoadAssetAtPath<BehaviorTypeRuntimeSetSO>(
            "Assets/ScriptableObjectsData/BehaviorSet.asset"
        );
}
