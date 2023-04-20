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
        private const float FLIP_COOLDOWN = .0f;
        private const float MIN_JUMP_FORCE = 1000f;
        private const float MAX_JUMP_FORCE = 6000f;
        private const float MIN_JUMP_COOLDOWN = 1f;
        private const float MAX_JUMP_COOLDOWN = 5f;

        private Rigidbody2D _rb;
        private bool _isInFlipCooldown = false;
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

            SetMoveAnimation(speed, canMove); // SET JUMP ANIMATION HERE
            StartCoroutine(StartJump(moveDirection));
        }

        IEnumerator StartJump(float moveDirection)
        {
            if (_isInJumpCooldown)
                yield break;

            _isInJumpCooldown = true;

            _rb.AddForce(new Vector2(0.8f * moveDirection, 1).normalized * _jumpForce);

            yield return new WaitForSeconds(_jumpCooldown);

            _isInJumpCooldown = false;
        }

        IEnumerator StartFlipCooldown()
        {
            _isInFlipCooldown = true;
            yield return new WaitForSeconds(FLIP_COOLDOWN);
            _isInFlipCooldown = false;
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

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) { }
        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }
        public override void Test()
        {
            Debug.Log("This is a JUMPER MOVEMENT Test");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("OnAir: " + IsOnAir());
            Debug.Log("!_isInFlipCooldown: " + !_isInFlipCooldown);
            Debug.Log("FloorTag: " +(col.gameObject.CompareTag("Floor")));
            if (col.gameObject.CompareTag("Floor") && !_isInFlipCooldown && IsOnAir())
            {
                StartCoroutine(StartFlipCooldown());
                _rb.velocity = new Vector2(_rb.velocity.x * (-1), _rb.velocity.y);
                base.VerifyOrientationAndFlip(0f, _mask);
            }
        }
    }
}
