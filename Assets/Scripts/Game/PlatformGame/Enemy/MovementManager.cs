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

        protected bool _flipRight = false;
        protected bool _flipLeft = false;
        protected bool _isFacingRight = true;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            OnFlip += Flip;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
        }

        public abstract void InitializeVariables();
        public abstract void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM);
        public abstract void Attack();
        public abstract void Victory();
        public abstract void Death();
        public abstract void Test();

        protected virtual void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            OnFlip?.Invoke();            
            //Debug.Log("R: " + _flipRight + " L: " + _flipLeft);
        }
        
        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
        
        /*
        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            var currentTransform = transform;
            currentTransform.Rotate(currentTransform.up, 180);
            var currentScale = currentTransform.localScale;
            currentTransform.localScale = new Vector3(currentScale.x, currentScale.y, -currentScale.z);
        }
        */
    }
}