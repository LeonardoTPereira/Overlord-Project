using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;
using UnityEngine.Events;

namespace PlatformGame.Enemy.Movement
{
    public class PatrolMovementManager : MovementManager
    {
        private const float FLIP_COOLDOWN = .5f;

        private Rigidbody2D _rb;
        private bool _isInFlipCooldown = false;
        private LayerMask _mask;
        private EdgeTester _edgeTester;

        public override void InitializeVariables()
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
            Debug.Log(col.gameObject.tag);
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
            Debug.Log(_rb.velocity);
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

        public override void Attack() { }
        public override void Victory() { }
        public override void Death() { }
        public override void Test()
        {
            Debug.Log("This is a RANDOM MOVEMENT Test");
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM) { }
    }
}