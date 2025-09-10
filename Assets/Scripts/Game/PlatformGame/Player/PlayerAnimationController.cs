using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator upPart;
        [SerializeField] private Animator downPart;

        public void AnimateMove(bool isRunning, float speed, bool canMove, bool isSeparated)
        {
            if (isSeparated)
            {
                if (canMove)
                {
                    SetAnimationParams(isRunning, speed);
                }
                else
                {
                    SetAnimationParams(false, 0f);
                }
            }
        }

        public void AnimateShoot()
        {
            upPart.SetTrigger("Attack");
        }
        
        private void SetAnimationParams(bool isRunning, float speed)
        {
            downPart.SetBool("IsRunning", isRunning);
            downPart.SetFloat("Speed", speed);
            upPart.SetBool("IsRunning", isRunning);
            upPart.SetFloat("Speed", speed);
        }
    }
}