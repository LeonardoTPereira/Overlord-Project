using UnityEditor;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Overlord-Project/Rules-Generator/EnemySO")]
    public class EnemySO : ScriptableObject, ISavableGeneratedContent
    {
        public float status1;
        public float status2;
        public float status3;
        public float status4;
        public float status5;
        public float status6;
        public float weaponStatus1;
        public float fitness;
        [SerializeField]
        public WeaponTypeSo weapon;
        [SerializeField]
        public MovementTypeSO movement;
        [SerializeField]
        public BehaviorTypeSO behavior;

        public void Init(float _status1, float _status2, float _status3, float _status4, float _status5, WeaponTypeSo _weapon,
            MovementTypeSO _movement, BehaviorTypeSO _behavior, float _fitness, float _status6, float _weaponStatus1)
        {
            status1 = _status1;
            status2 = _status2;
            status3 = _status3;
            status4 = _status4;
            status5 = _status5;
            weapon = _weapon;
            movement = _movement;
            behavior = _behavior;
            fitness = _fitness;
            status6 = _status6;
            weaponStatus1 = _weaponStatus1;
        }
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string newFolder = "Enemies";
            var fileName = directory;
            if (!AssetDatabase.IsValidFolder(fileName + Constants.SeparatorCharacter + newFolder))
            {
                AssetDatabase.CreateFolder(fileName, newFolder);
            }
            fileName += Constants.SeparatorCharacter + newFolder;
            fileName += Constants.SeparatorCharacter;
            fileName += weapon + ".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
#endif
        }
    }
}