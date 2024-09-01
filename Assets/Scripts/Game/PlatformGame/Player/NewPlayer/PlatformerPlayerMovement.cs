using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformGame.Player
{
    public class PlatformerPlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationController _animController;
        public CharacterController2D controller;

        public float runSpeed = 40f;

        float horizontalMove = 0f;
        bool _pressedJump = false;

        public void PressedMovement(InputAction.CallbackContext context)
        {
            horizontalMove = context.ReadValue<float>() * runSpeed;
        }

        public void PressJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _pressedJump = true;
            }
        }

        void FixedUpdate()
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, _pressedJump);
            _animController.AnimateMove(false, Mathf.Abs(horizontalMove), true, true);
            _pressedJump = false;
        }
    }
}