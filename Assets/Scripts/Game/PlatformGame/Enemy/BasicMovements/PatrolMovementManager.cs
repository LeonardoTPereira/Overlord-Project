using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;
using UnityEngine.Events;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class PatrolMovementManager : MovementManager
    {
        private Rigidbody2D _rb;
        private LayerMask _mask;
        private EdgeTester _edgeTester;

        public override void InitializeVariables(EnemySO enemySo)
        {
            _rb = GetComponent<Rigidbody2D>();
            _edgeTester = transform.GetChild(0).gameObject.GetComponent<EdgeTester>();
            _edgeTester.edgeTesterOnTriggerExit.AddListener(OnTheOtherTriggerExitMethod);
        }

        private void OnDisable()
        {
            _edgeTester.edgeTesterOnTriggerExit.RemoveListener(OnTheOtherTriggerExitMethod);
        }

        private void OnTheOtherTriggerExitMethod(Collider2D col)
        {
            if (col.gameObject.tag == "Floor" && !_isInFlipCooldown)
            {
                StartCoroutine(StartFlipCooldown());
                base.VerifyOrientationAndFlip(0f, _mask);
            }
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            moveDirection = -1f;
            if (_isFacingRight)
                moveDirection = 1f;

            SetMoveAnimation(speed, canMove);
            _rb.velocity = new Vector2(moveDirection * speed, _rb.velocity.y);
        }

        private void SetMoveAnimation(float speed, bool canMove)
        {
            _animation.AnimateMove(false, speed, canMove);
        }

        public override void Test()
        {
            Debug.Log("This is a RANDOM MOVEMENT Test");
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) { }
    }
}