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
        private Controller _controller;

        private void Start()
        {
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            //AnimateMove(_controller.);
        }

        public void AnimateMove(bool isRunning, float speed, bool canMove, bool isSeparated)
        {
            if (isSeparated)
            {
                if (canMove)
                {
                    downPart.SetBool("IsRunning", isRunning);
                    downPart.SetFloat("Speed", speed);
                    upPart.SetBool("IsRunning", isRunning);
                    upPart.SetFloat("Speed", speed);
                }
                else
                {
                    downPart.SetBool("IsRunning", false);
                    downPart.SetFloat("Speed", 0f);
                    upPart.SetBool("IsRunning", false);
                    upPart.SetFloat("Speed", 0f);
                }
            }
        }

        public void AnimateShoot()
        {
            upPart.SetTrigger("Attack");
        }
    
    }
}
