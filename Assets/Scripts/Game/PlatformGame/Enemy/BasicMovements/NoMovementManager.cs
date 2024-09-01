using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

namespace PlatformGame.Enemy.Movement
{
    public class NoMovementManager : MovementManager
    {
        public override void InitializeVariables(EnemySO enemySo) { }
        public override void Move(float moveDirection, float speed, bool canMove, LayerMask groundLM) 
        {
            VerifyOrientationAndFlip(moveDirection, groundLM);
        }

        public override void Test()
        {
            Debug.Log("This is a NO MOVEMENT Test");
        }
        protected override void VerifyOrientationAndFlip(float moveDirection, LayerMask groundLM)
        {
            if (moveDirection < 0 && _isFacingRight && !_flipLeft)
            {
                _flipLeft = true;
                _flipRight = false;
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
            }
            else if (moveDirection > 0 && !_isFacingRight && !_flipRight)
            {
                _flipLeft = false;
                _flipRight = true;
                base.VerifyOrientationAndFlip(moveDirection, groundLM);
            }
        }
    }
}
