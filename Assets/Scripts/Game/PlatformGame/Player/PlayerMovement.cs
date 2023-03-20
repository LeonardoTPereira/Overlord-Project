using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformGame.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float moveDirection;
        private bool isGrounded;
        private bool isFacingRight;
        [SerializeField] private float speed = 4f;
        [SerializeField] private float runSpeedMultiplier = 1.5f;
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private bool isSeparated = false;
        private bool isRunning;
        private Rigidbody2D _rb;
        private bool _canMove;

        public static event Action OnFlip;

        private void OnEnable()
        {
            OnFlip += Flip;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
        }

        public void PressedMovement(InputAction.CallbackContext context)
        {
            moveDirection = context.ReadValue<float>();
            if (moveDirection > 0 && isFacingRight)
            {
                OnFlip?.Invoke();
            }
            else if (moveDirection < 0 && !isFacingRight)
            {
                OnFlip?.Invoke();
            }
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.transform.CompareTag("Floor"))
            {
                isGrounded = true;
            }
        }
        /*
        private void FixedUpdate()
        {
                if (moveDirection < 0 && isFacingRight)
                {
                    OnFlip?.Invoke();
                }
                if (moveDirection > 0 && !isFacingRight)
                {
                    OnFlip?.Invoke();
                }
            //playerAnimation.AnimateMove(isRunning, Mathf.Abs(moveDirection), _canMove, isSeparated);
        }
        */
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            isGrounded = true;
            isFacingRight = true;
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }        
    }
}

