using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Enemy.Movement
{
    public class WolfFollowMovementManager : WolfMovementManager
    {
        private float _moveDirection;
        private Rigidbody2D _rb;
        private GameObject _player;
        private bool _canMove;

        private void Awake()
        {
            // After, use timer to set enemy freeze for some seconds
            _rb = GetComponent<Rigidbody2D>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (!_canMove)
                return;
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            _moveDirection = (transform.position - _player.transform.position).x;
        }

        public override void Move(float speed, bool canMove)
        {
            if (_canMove)
            {
                _animator.SetBool("IsRunning", false);
                _animator.SetFloat("Speed", speed);
            }
            else
            {
                _animator.SetBool("IsRunning", false);
                _animator.SetFloat("Speed", 0f);
            }
            _rb.transform.position += Vector3.right * _moveDirection * Time.fixedDeltaTime * speed;
        }

        public override void Test()
        {
            Debug.Log("This is a Wolf FOLLOW PLAYER Test");
        }
    }
}
