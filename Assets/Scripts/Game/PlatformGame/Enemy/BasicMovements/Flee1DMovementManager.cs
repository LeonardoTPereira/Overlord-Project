using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public class Flee1DMovementManager : MovementManager
    {
        private Rigidbody2D _rb;

        public override void InitializeVariables()
        {
            // After, use timer to set enemy freeze for some seconds
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;
            SetMoveAnimation(speed, canMove);
            VerifyOrientationAndFlip(moveDirection, groundLM);
            _rb.velocity = new Vector2((-1)*moveDirection * speed, _rb.velocity.y);
            //_rb.transform.position += Vector3.right * (-1)*_moveDirection * Time.fixedDeltaTime * speed;
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
            Debug.Log("This is a Wolf FOLLOW PLAYER Test");
        }

        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            if ((-1)* moveDirection < 0 && _isFacingRight && !_flipLeft)
            {
                _flipLeft = true;
                _flipRight = false;
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
            }
            else if ((-1)* moveDirection > 0 && !_isFacingRight && !_flipRight)
            {
                _flipLeft = false;
                _flipRight = true;
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
            }
        }
    }
}