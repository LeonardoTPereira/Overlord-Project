using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public abstract class MovementManager : MonoBehaviour
    {
        public event Action OnFlip;
        protected const float FLIP_COOLDOWN = 0.5f;
        protected bool _flipRight = false;
        protected bool _flipLeft = false;
        protected bool _isFacingRight = true;
        protected bool _isInFlipCooldown = false;
        protected EnemyAnimation _animation;

        private void Awake()
        {
            _animation = GetComponent<EnemyAnimation>();
        }

        private void OnEnable()
        {
            OnFlip += Flip;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
        }

        public abstract void InitializeVariables(EnemySO enemySo);
        public abstract void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM);
        public abstract void Test();

        protected virtual void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            OnFlip?.Invoke();            
        }

        protected IEnumerator StartFlipCooldown()
        {
            if (_isInFlipCooldown)
                yield break;
            _isInFlipCooldown = true;
            yield return new WaitForSeconds(FLIP_COOLDOWN);
            _isInFlipCooldown = false;
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
}