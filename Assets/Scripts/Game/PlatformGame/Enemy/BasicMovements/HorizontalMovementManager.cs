using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public class HorizontalMovementManager : MovementManager
    {
        private const float MAX_HORIZONTAL_DIST = 3f;
        private const float MAX_VERTICAL_DIST = 10f;
        private const float FLIP_COOLDOWN = .5f;
        private const float X_OFFSET = 1.0f;
        private const float Y_OFFSET = 1.0f;

        private Rigidbody2D _rb;
        private bool _isInFlipCooldown = false;
        private LayerMask _mask;
        public override void InitializeVariables()
        {
            // After, use timer to set enemy freeze for some seconds
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;

            moveDirection = -1f;
            if (_isFacingRight)
                moveDirection = 1f;

            VerifyOrientationAndFlip(moveDirection, groundLM);
            SetMoveAnimation(speed, canMove);
            _rb.velocity = new Vector2(0, 0);
            _rb.velocity = new Vector2(moveDirection * speed, _rb.velocity.y);
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
        /*
        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            if (!_isInFlipCooldown)
            {
                var currentTransform = transform;
                //Vector3 testPos = new Vector3(transform.position.x, 1f, transform.position.z);
                Vector3 directionPos = transform.right;
                if (!_isFacingRight)
                    directionPos *= -1;

                bool hasNotGround = (Physics2D.Raycast(currentTransform.position + directionPos * MAX_HORIZONTAL_DIST,
                    -currentTransform.up,
                    MAX_VERTICAL_DIST, groundLM).collider == null);

                bool hasFrontBlock = (Physics2D.Raycast(currentTransform.position + new Vector3(0, Y_OFFSET, 0),
                    currentTransform.position + directionPos * MAX_HORIZONTAL_DIST + new Vector3(0, Y_OFFSET, 0),
                    MAX_HORIZONTAL_DIST, groundLM).collider != null);
                //Debug.Log("hasNotGround: " + hasNotGround + " hasFrontBlock: " + hasFrontBlock);
                //Debug.Log(directionPos);

                Vector3 test = currentTransform.position + new Vector3(0, Y_OFFSET, 0);
                //Debug.Log("Origin transform: " + test);
                Vector3 test2 = currentTransform.position + directionPos + new Vector3(0, Y_OFFSET, 0);
                //Debug.Log("Destiny transform: " + test2);


                if (hasNotGround || hasFrontBlock)
                {
                    //Debug.Log("hasNotGround: " + hasNotGround + " hasFrontBlock: " + hasFrontBlock);
                    StartCoroutine(StartFlipCooldown());
                    base.VerifyOrientationAndFlip(moveDirection, groundLM);
                }
            }
        }*/

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            if (!_isInFlipCooldown)
            {
                Vector2 currTrans = new Vector2(transform.position.x, transform.position.y);
                //Vector3 testPos = new Vector3(transform.position.x, 1f, transform.position.z);
                Vector2 directionPos = new Vector2(1, 0);
                if (!_isFacingRight)
                    directionPos *= -1;
                Debug.DrawLine(currTrans + directionPos * new Vector2(6.0f, 0), currTrans + new Vector2(0, -3.0f), Color.red);
                bool hasNotGround = (Physics2D.Raycast(currTrans + directionPos * new Vector2(6.0f, 0),
                    currTrans + new Vector2(0, -3.0f),
                    MAX_VERTICAL_DIST, groundLM).collider == null);

                bool hasFrontBlock = (Physics2D.Raycast(currTrans + new Vector2(0, Y_OFFSET),
                    currTrans + directionPos * MAX_HORIZONTAL_DIST + new Vector2(0, Y_OFFSET),
                    MAX_HORIZONTAL_DIST, groundLM).collider != null);
                Debug.DrawLine(currTrans + new Vector2(0, Y_OFFSET), currTrans + directionPos * MAX_HORIZONTAL_DIST + new Vector2(0, Y_OFFSET), Color.blue);
                Debug.Log("hasNotGround: " + hasNotGround + " hasFrontBlock: " + hasFrontBlock);

                if (hasNotGround || hasFrontBlock)
                {
                    StartCoroutine(StartFlipCooldown());
                    base.VerifyOrientationAndFlip(moveDirection, groundLM);
                }
            }
        }

        IEnumerator StartFlipCooldown()
        {
            _isInFlipCooldown = true;
            yield return new WaitForSeconds(FLIP_COOLDOWN);
            _isInFlipCooldown = false;
        }
    }
}