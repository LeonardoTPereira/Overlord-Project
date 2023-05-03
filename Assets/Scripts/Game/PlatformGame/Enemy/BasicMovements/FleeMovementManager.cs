using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class FleeMovementManager : MovementManager
    {
        private Rigidbody2D _rb;

        public override void InitializeVariables(EnemySO enemySO)
        {
            // After, use timer to set enemy freeze for some seconds
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM)
        {
            if (!canMove)
                return;
            SetMoveAnimation(speed, canMove);
            _rb.velocity = new Vector2((-1)*moveDirection * speed, _rb.velocity.y);
            //_rb.transform.position += Vector3.right * (-1)*_moveDirection * Time.fixedDeltaTime * speed;
        }

        private void SetMoveAnimation(float speed, bool canMove)
        {
            _animation.AnimateMove(false, Mathf.Abs(speed), canMove);
        }

        public override void Test()
        {
            Debug.Log("This is a Wolf FOLLOW PLAYER Test");
        }
    }
}


