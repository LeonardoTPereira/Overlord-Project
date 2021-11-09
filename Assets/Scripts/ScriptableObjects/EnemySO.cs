using UnityEngine;

namespace ScriptableObjects
{
    public class EnemySO : ScriptableObject
    {
        public int health;
        public int damage;
        public float movementSpeed;
        public float activeTime;
        public float restTime;
        [SerializeField]
        public WeaponTypeSO weapon;
        [SerializeField]
        public MovementTypeSO movement;
        [SerializeField]
        public BehaviorTypeSO behavior;
        public float fitness;
        public float attackSpeed;
        public float projectileSpeed;

        public void Init(int _health, int _damage, float _movementSpeed, float _activeTime, float _restTime, WeaponTypeSO _weapon,
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
    }
}