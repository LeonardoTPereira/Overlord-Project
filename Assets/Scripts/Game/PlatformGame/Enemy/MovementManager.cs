using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public abstract class MovementManager : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }
        public abstract void Move(float speed, bool canMove);
        public abstract void Attack();
        public abstract void Victory();
        public abstract void Death();
        public abstract void Test();
    }
}
