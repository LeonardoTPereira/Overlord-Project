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
        private const float MIN_JUMP_FORCE = 2000f;
        private const float MAX_JUMP_FORCE = 3500f;
        private const float MIN_JUMP_COOLDOWN = 1f;
        private const float MAX_JUMP_COOLDOWN = 3f;

        private Rigidbody2D _rb;
        private bool _isInJumpCooldown = false;
        private LayerMask _mask;
        private float _jumpForce = MIN_JUMP_FORCE;
        private float _jumpCooldown = 2f;

        public override void InitializeVariables(EnemySO enemySo)
        {
            _rb = GetComponent<Rigidbody2D>();
            _jumpCooldown = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(4f - enemySo.movementSpeed, MIN_JUMP_COOLDOWN, MAX_JUMP_COOLDOWN, .8f, 3.2f);
            _jumpForce = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.projectileSpeed, MIN_JUMP_FORCE, MAX_JUMP_FORCE, 1f, 4f);
            Debug.Log("Jump Cooldown: " + _jumpCooldown + " Force: " + _jumpForce);
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
            _animation.AnimateMove(false, Mathf.Abs(speed), canMove);
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) 
        {
            if (_isFacingRight && moveDirection < 0f || !_isFacingRight && moveDirection > 0f)
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
        }
        public override void Test()
        {
            Debug.Log("This is a JUMPER UP MOVEMENT Test");
        }
    }
}

