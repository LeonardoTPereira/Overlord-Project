using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameManager.Player
{
    public class PlayerMovement : PlayerInput
    {

        private bool _canMove;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float speed;
        private Vector2 lastFacingVector;
        private static readonly int LastDirX = Animator.StringToHash("LastDirX");
        private static readonly int LastDirY = Animator.StringToHash("LastDirY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        public void Move(InputAction.CallbackContext context)
        {
            if (!_canMove) return;
            var inputX = context.ReadValue<Vector2>().x;
            var inputY = context.ReadValue<Vector2>().y;
            var movement = new Vector2(inputX, inputY);
            movement.Normalize();
            if (movement.magnitude > 0.01f)
                _rigidbody2D.velocity = new Vector2(movement.x * speed, movement.y * speed);
            else
                _rigidbody2D.velocity = Vector3.zero;
            UpdateMoveAnimation(movement);
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
                    PlayerAnimator.SetFloat(LastDirX, lastFacingVector.x);
                    PlayerAnimator.SetFloat(LastDirY, lastFacingVector.y);
                }
                PlayerAnimator.SetBool(IsMoving, false);
            }
            //Else, update the idle direction
            else
            {
                lastFacingVector.x = movement.x;
                lastFacingVector.y = movement.y;
                PlayerAnimator.SetFloat(DirX, movement.x);
                PlayerAnimator.SetFloat(DirY, movement.y);
                PlayerAnimator.SetBool(IsMoving, true);
            }
        }
    }
}