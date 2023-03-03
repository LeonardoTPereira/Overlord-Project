using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy
{
    public class WolfEnemyAnimation : EnemyAnimation
    {
        [SerializeField] private Animator _wolfAnimator;

        private void Awake()
        {
            if(_wolfAnimator==null)
                _wolfAnimator = GetComponent<Animator>();
        }

        public override void AnimateMove(bool isRunning, float speed, bool canMove)
        {
            if (canMove)
            {
                _wolfAnimator.SetBool("IsRunning", isRunning);
                _wolfAnimator.SetFloat("Speed", speed);
            }
            else
            {
                _wolfAnimator.SetBool("IsRunning", false);
                _wolfAnimator.SetFloat("Speed", 0f);
            }
        
        }

        public override void AnimateAttack()
        {
            _wolfAnimator.SetTrigger("Attack");
        }

        public override void AnimateVictory()
        {
            _wolfAnimator.SetTrigger("Victory");
        }
        
        public override void AnimateDeath()
        {
            _wolfAnimator.SetTrigger("Die");
        }
    }
}