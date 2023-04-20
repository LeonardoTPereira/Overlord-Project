using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;
using UnityEngine.Events;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class CooldownPatrolMovementManager : MovementManager
    {
        private const float FLIP_DEFAULT_COOLDOWN = .5f;            
        private const float FLIP_MIN_COOLDOWN = 2f;
        private const float FLIP_MAX_COOLDOWN = 8f;

        private Rigidbody2D _rb;
        private LayerMask _mask;
        private EdgeTester _edgeTester;

        private float _flipCooldown = FLIP_MIN_COOLDOWN;
        private bool _isFlipDefaultCooldown = false;
        private bool _isFlipCooldown = false;

        public override void InitializeVariables(EnemySO enemySo)
        {
            _rb = GetComponent<Rigidbody2D>();
            _edgeTester = transform.GetChild(0).gameObject.GetComponent<EdgeTester>();
            _edgeTester.edgeTesterOnTriggerExit.AddListener(OnTheOtherTriggerExitMethod);

            _flipCooldown = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.attackSpeed, FLIP_MIN_COOLDOWN, FLIP_MAX_COOLDOWN, 1f, 4f);
        }

        private void OnDisable()
        {
            _edgeTester.edgeTesterOnTriggerExit.RemoveListener(OnTheOtherTriggerExitMethod);
        }

        private void OnTheOtherTriggerExitMethod(Collider2D col)
        {
            if (col.gameObject.tag == "Floor" && !_isFlipDefaultCooldown)
            {
                StartCoroutine(StartFlipDefaultCooldown());
                base.VerifyOrientationAndFlip(0f, _mask);
            }
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            if (!_isFlipDefaultCooldown && !_isFlipCooldown)
                VerifyOrientationAndFlip(moveDirection, groundLM);
            
            moveDirection = -1f;
            if (_isFacingRight)
                moveDirection = 1f;
            
            SetMoveAnimation(speed, canMove);
            _rb.velocity = new Vector2(moveDirection * speed, _rb.velocity.y);
        }

        // OBS: Bug related to flip when the enemy hits a wall (it call StartFlipDefaultCooldown and, if StartFlipCooldown ends and sets 
        // _isFlipCooldown to false, the enemy flips twice and the enemy keeps in front of the wall
        // (flips by OnTheOtherTriggerExitMethod and Move line ~53)
        IEnumerator StartFlipCooldown()
        {
            if (_isFlipCooldown)
                yield break;

            _isFlipCooldown = true;

            yield return new WaitForSeconds(_flipCooldown);

            _isFlipCooldown = false;
        }

        IEnumerator StartFlipDefaultCooldown()
        {
            if (_isFlipDefaultCooldown)
                yield break;

            _isFlipDefaultCooldown = true;
            _isFlipCooldown = false;

            yield return new WaitForSeconds(FLIP_DEFAULT_COOLDOWN);

            _isFlipDefaultCooldown = false;
        }
        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) 
        {
            StartCoroutine(StartFlipCooldown());
            base.VerifyOrientationAndFlip(moveDirection, _mask);
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

        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }
        public override void Test()
        {
            Debug.Log("This is a RANDOM MOVEMENT Test");
        }

    }
}