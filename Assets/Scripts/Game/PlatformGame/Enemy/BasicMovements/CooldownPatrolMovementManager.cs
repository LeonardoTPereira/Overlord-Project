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
        private const float FLIP_MAX_COOLDOWN = 5.5f;

        private Rigidbody2D _rb;
        private LayerMask _mask;
        //private EdgeTester _edgeTester;

        private float _flipCooldown = FLIP_MIN_COOLDOWN;
        private bool _isFlipDefaultCooldown = false;

        public override void InitializeVariables(EnemySO enemySo)
        {
            _rb = GetComponent<Rigidbody2D>();
            //_edgeTester = transform.GetChild(0).gameObject.GetComponent<EdgeTester>();
            //_edgeTester.edgeTesterOnTriggerExit.AddListener(OnTheOtherTriggerExitMethod);

            _flipCooldown = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.attackSpeed, FLIP_MIN_COOLDOWN, FLIP_MAX_COOLDOWN, 0.75f, 4f);
            Debug.Log(_flipCooldown);
        }

        private void OnDisable()
        {
            //_edgeTester.edgeTesterOnTriggerExit.RemoveListener(OnTheOtherTriggerExitMethod);
        }

        /*
        private void OnTheOtherTriggerExitMethod(Collider2D col)
        {
            if (col.gameObject.tag == "Floor" && !_isFlipDefaultCooldown)
            {
                // Reset move cooldown and move
                base.VerifyOrientationAndFlip(0f, _mask);
            }
        }
        */

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            if (!_isFlipDefaultCooldown)
                VerifyOrientationAndFlip(moveDirection, groundLM);

            StartCoroutine(StartFlipDefaultCooldown());

            moveDirection = -1f;
            if (_isFacingRight)
                moveDirection = 1f;
            
            SetMoveAnimation(speed, canMove);
            _rb.velocity = new Vector2(moveDirection * speed, _rb.velocity.y);
        }

        IEnumerator StartFlipDefaultCooldown()
        {
            if (_isFlipDefaultCooldown)
                yield break;

            _isFlipDefaultCooldown = true;

            yield return new WaitForSeconds(_flipCooldown);

            _isFlipDefaultCooldown = false;
        }
        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) 
        {
            base.VerifyOrientationAndFlip(moveDirection, _mask);
        }

        private void SetMoveAnimation(float speed, bool canMove)
        {
            _animation.AnimateMove(false, speed, canMove);
        }

        public override void Test()
        {
            Debug.Log("This is a RANDOM MOVEMENT Test");
        }

    }
}