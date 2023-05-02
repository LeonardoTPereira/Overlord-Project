using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy
{
    public class DefaultEnemyAnimation : EnemyAnimation
    {
        [SerializeField] protected Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        public override void AnimateMove(bool isRunning, float speed, bool canMove)
        {
            if (canMove)
            {
                _animator.SetBool("IsRunning", isRunning);
                _animator.SetFloat("Speed", speed);
            }
            else
            {
                _animator.SetBool("IsRunning", false);
                _animator.SetFloat("Speed", 0f);
            }
        }

        public override void AnimateAttack()
        {
            _animator.SetTrigger("Attack");
        }

        public override void AnimateVictory()
        {
            _animator.SetTrigger("Victory");
        }

        public override void AnimateDeath()
        {
            _animator.SetTrigger("Die");
        }
    }
}