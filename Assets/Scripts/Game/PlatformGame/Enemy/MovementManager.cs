using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlatformGame.Enemy.Movement
{
    public abstract class MovementManager : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        public event Action OnFlip;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }

        public abstract void InitializeVariables();
        public abstract void Move(float moveDirection, float speed, bool canMove);
        public abstract void Attack();
        public abstract void Victory();
        public abstract void Death();
        public abstract void Test();
    }
}