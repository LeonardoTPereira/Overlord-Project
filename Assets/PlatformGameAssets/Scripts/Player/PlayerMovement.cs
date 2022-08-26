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
        private PlayerAnimationController playerAnimation;

        public static event Action OnFlip;

        private void OnEnable()
        {
            OnFlip += Flip;
            PlayerShots.IsShooting += DisableInput;
            PlayerShots.StopShooting += EnableInput;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
            PlayerShots.IsShooting -= DisableInput;
            PlayerShots.StopShooting -= EnableInput;
        }

        private void Awake()
        {
            _canMove = true;
        }

        private void OnMove(InputValue value)
        {
            moveDirection = value.Get<float>();
        }

        private void OnRun(InputValue value)
        {
            isRunning = value.isPressed;
        }

        private void OnJump()
        {
            if (isGrounded&&_canMove)
            {
                _rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.transform.CompareTag("Floor"))
            {
                isGrounded = true;
            }
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                if (moveDirection < 0 && isFacingRight)
                {
                    OnFlip?.Invoke();
                }
                if (moveDirection > 0 && !isFacingRight)
                {
                    OnFlip?.Invoke();
                }
                
                _rb.transform.position += Vector3.right*moveDirection*Time.fixedDeltaTime*speed*(isRunning?(runSpeedMultiplier):(1));
            }
            playerAnimation.AnimateMove(isRunning, Mathf.Abs(moveDirection), _canMove, isSeparated);
        }
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            playerAnimation = GetComponent<PlayerAnimationController>();
            isGrounded = true;
            isFacingRight = true;
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            //transform.Rotate(0f, 180f, 0f, Space.Self);
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
        
        private void EnableInput()
        {
            _canMove = true;
        }
        
        private void DisableInput()
        {
            _canMove = false;
        }
        
    }
}

