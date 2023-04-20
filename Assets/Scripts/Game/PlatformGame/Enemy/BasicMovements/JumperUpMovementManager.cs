using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;
using UnityEngine.Events;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class JumperUpMovementManager : MovementManager
    {
        private const float MIN_JUMP_FORCE = 1000f;
        private const float MAX_JUMP_FORCE = 6000f;
        private const float MIN_JUMP_COOLDOWN = 1f;
        private const float MAX_JUMP_COOLDOWN = 5f;

        private Rigidbody2D _rb;
        private bool _isInJumpCooldown = false;
        private LayerMask _mask;
        private float _jumpForce = MIN_JUMP_FORCE;
        private float _jumpCooldown = 2f;

        public override void InitializeVariables(EnemySO enemySo)
        {
            _rb = GetComponent<Rigidbody2D>();
            _jumpCooldown = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.movementSpeed, MIN_JUMP_FORCE, MAX_JUMP_FORCE, .8f, 3.2f);
            _jumpForce = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.projectileSpeed, MIN_JUMP_FORCE, MAX_JUMP_FORCE, 1f, 4f);
            _jumpForce = MIN_JUMP_FORCE;
            _jumpForce = MAX_JUMP_FORCE;
            _jumpCooldown = MIN_JUMP_COOLDOWN;
            //_jumpCooldown = MAX_JUMP_COOLDOWN;
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            VerifyOrientationAndFlip(moveDirection, groundLM);
            SetMoveAnimation(speed, canMove); // SET JUMP ANIMATION HERE
            StartCoroutine(StartJump(moveDirection));
        }

        IEnumerator StartJump(float moveDirection)
        {
            if (_isInJumpCooldown)
                yield break;

            _isInJumpCooldown = true;

            _rb.AddForce(new Vector2(0, 1) * _jumpForce);

            yield return new WaitForSeconds(_jumpCooldown);

            _isInJumpCooldown = false;
        }

        private void SetMoveAnimation(float speed, bool canMove)
        {
            if (canMove)
            {
                _animator.SetBool("IsRunning", false);
                _animator.SetFloat("Speed", Mathf.Abs(speed));
            }
            else
            {
                _animator.SetBool("IsRunning", false);
                _animator.SetFloat("Speed", 0f);
            }
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) 
        {
            if (_isFacingRight && moveDirection < 0f || !_isFacingRight && moveDirection > 0f)
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
        }
        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }
        public override void Test()
        {
            Debug.Log("This is a JUMPER UP MOVEMENT Test");
        }
    }
}

