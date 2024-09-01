using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Archer
{
    public class ArcherEnemyAnimation : EnemyAnimation
    {
        [SerializeField] private Animator upPart;
        [SerializeField] private Animator downPart;

        public override void AnimateMove(bool isRunning, float speed, bool canMove)
        {
            if (canMove)
            {
                downPart.SetBool("IsRunning", isRunning);
                downPart.SetFloat("Speed", speed);
            }
            else
            {
                downPart.SetBool("IsRunning", false);
                downPart.SetFloat("Speed", 0f);
            }
        
        }

        public override void AnimateAttack()
        {
            upPart.SetTrigger("Attack");
        }
        
        public override void AnimateVictory()
        {
        }
        
        public override void AnimateDeath()
        {
        }
    }

}