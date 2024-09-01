using UnityEditor;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO")]
    public class EnemySO : ScriptableObject, ISavableGeneratedContent
    {
        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        [SerializeField]
        public WeaponTypeSo weapon;
        [SerializeField]
        public MovementTypeSO movement;
        [SerializeField]
        public BehaviorTypeSO behavior;
        public float fitness;
        public float attackSpeed;
        public float projectileSpeed;

        public void Init(int _health, int _damage, float _movementSpeed, float _activeTime, float _restTime, WeaponTypeSo _weapon,
            MovementTypeSO _movement, BehaviorTypeSO _behavior, float _fitness, float _attackSpeed, float _projectileSpeed)
        {
            health = _health;
            damage = _damage;
            movementSpeed = _movementSpeed;
            activeTime = _activeTime;
            restTime = _restTime;
            weapon = _weapon;
            movement = _movement;
            behavior = _behavior;
            fitness = _fitness;
            attackSpeed = _attackSpeed;
            projectileSpeed = _projectileSpeed;
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
            fileName += weapon+".asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
#endif
        }
    }
}