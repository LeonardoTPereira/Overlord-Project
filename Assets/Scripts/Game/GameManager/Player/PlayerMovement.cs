using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Game.GameManager.Player
{
    public class PlayerMovement : PlayerInputHandler
    {

        private bool _canMove;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float speed;
        private Vector2 _lastFacingVector;
        private Vector2 _lastSpeed;
        private static readonly int LastDirX = Animator.StringToHash("LastDirX");
        private static readonly int LastDirY = Animator.StringToHash("LastDirY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        public void Move(InputAction.CallbackContext context)
        {
	        if (context.canceled)
	        {
		        _lastSpeed = Vector2.zero;
		        return;
	        }
	        if (!_canMove) return;
	        if (!context.performed) return;
	        var movement = context.ReadValue<Vector2>();
	        movement.Normalize();
	        _lastSpeed = new Vector2(movement.x * speed, movement.y * speed);
	        UpdateMoveAnimation(movement);
        }

        private void FixedUpdate()
        {
	        _rigidbody2D.velocity = _lastSpeed;
        }

        private void Awake()
        {
            _canMove = true;
        }

        protected override void Start()
        {
            base.Start();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        protected override void StartInput(object sender, EventArgs eventArgs)
        {
            _canMove = true;
        }

        protected override void StopInput(object sender, EventArgs eventArgs)
        {
            _canMove = false;
            _rigidbody2D.velocity = Vector3.zero;
        }


        private void UpdateMoveAnimation(Vector2 movement)
        {
            //If not shooting nor moving, maintain the idle direction
            if (movement.x == 0f && movement.y == 0f)
            {
                if (!PlayerAnimator.GetBool(IsShooting))
                {
                    PlayerAnimator.SetFloat(LastDirX, _lastFacingVector.x);
                    PlayerAnimator.SetFloat(LastDirY, _lastFacingVector.y);
                }
                PlayerAnimator.SetBool(IsMoving, false);
            }
            //Else, update the idle direction
            else
            {
                _lastFacingVector.x = movement.x;
                _lastFacingVector.y = movement.y;
                PlayerAnimator.SetFloat(DirX, movement.x);
                PlayerAnimator.SetFloat(DirY, movement.y);
                PlayerAnimator.SetBool(IsMoving, true);
            }
        }
    }
}