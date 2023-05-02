using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy
{
    public class InfectedAnt1Animation : DefaultEnemyAnimation
    {
        public override void AnimateMove(bool isRunning, float speed, bool canMove)
        {
            if (canMove)
            {
                _animator.SetFloat("Speed", speed);
            }
            else
            {
                _animator.SetFloat("Speed", 0f);
            }
        }
    }
}