using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;
using UnityEngine.Events;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class JumperMovementManager : MovementManager
    {
        private const float MIN_JUMP_FORCE = 1500f;
        private const float MAX_JUMP_FORCE = 3000f;
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
            _jumpCooldown = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(4f - enemySo.movementSpeed, MIN_JUMP_COOLDOWN, MAX_JUMP_COOLDOWN, .8f, 3.2f);
            _jumpForce = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.projectileSpeed, MIN_JUMP_FORCE, MAX_JUMP_FORCE, 1f, 4f);
        }

        private bool IsOnAir()
        {
            return Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            moveDirection = -1f;
            if (_isFacingRight)
                moveDirection = 1f;

           // _animation.; // SET JUMP ANIMATION HERE
            StartCoroutine(StartJump(moveDirection));
        }

        IEnumerator StartJump(float moveDirection)
        {
            if (_isInJumpCooldown)
                yield break;

            _isInJumpCooldown = true;

            _rb.AddForce(new Vector2(0.5f * moveDirection, 1).normalized * _jumpForce);

            yield return new WaitForSeconds(_jumpCooldown);

            _isInJumpCooldown = false;
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) { }
        public override void Test()
        {
            Debug.Log("This is a JUMPER MOVEMENT Test");
        }

        private void DebugLogs(Collider2D col)
        {
            Debug.Log("OnAir: " + IsOnAir());
            Debug.Log("!_isInFlipCooldown: " + !_isInFlipCooldown);
            Debug.Log("FloorTag: " + (col.gameObject.CompareTag("Floor")));
            Debug.Log("Collider obj name: " + col.gameObject.name);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //DebugLogs(col);
            if (col.gameObject.CompareTag("Floor") && !_isInFlipCooldown && IsOnAir())
            {
                DebugLogs( col);
                StartCoroutine(StartFlipCooldown());
                base.VerifyOrientationAndFlip(0f, _mask);
            }
        }
    }
}
