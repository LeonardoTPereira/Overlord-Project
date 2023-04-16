using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlatformGame.Player;
using ScriptableObjects;
using PlatformGame.Util;
using System.ComponentModel;
using Util;
using PlatformGame.Enemy.Movement;

namespace PlatformGame.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float _minimumSpeed = 0.8f;
        [SerializeField] private float _maximumSpeed = 3.2f;

        public event Action OnFlip;
        
        private float _moveDirection;
        private bool _isFacingRight;
        private bool _canMove;

        private GameObject _player;

        private float _speed;

        [HideInInspector] public MovementManager moveManager;
        protected bool _itFlips = true;
        protected bool _flipsInOpositeDirection = false;

        public void LoadMovement(EnemySO enemySo)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _isFacingRight = true;
            // From 0.8f to 3.2f in SearchSpace
            _speed = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.movementSpeed, _minimumSpeed, _maximumSpeed, .8f, 3.2f);

            // This movement doesn't flip to player direction
            if (enemySo.movement.enemyMovementIndex == Enums.MovementEnum.Follow1D)
                _itFlips = false;

            GenerateMovementComponent(enemySo.movement.enemyMovementIndex);

            moveManager.InitializeVariables();
            moveManager.Test();
        }
        private void UpdateDirection()
        {
            if (!_canMove)
                return;
            _moveDirection = (_player.transform.position - transform.position).x;
            if (_moveDirection > 0)
                _moveDirection = 1;
            else
                _moveDirection = -1;
        }

        private void OnEnable()
        {
            OnFlip += Flip;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
        }

        protected virtual void GenerateMovementComponent(Enums.MovementEnum moveEnum) { }

        public void UpdateMovement()
        {
            UpdateDirection();
            VerifyOrientationAndFlip();
            moveManager.Move(_moveDirection, _speed, _canMove);
        }

        private void VerifyOrientationAndFlip()
        {
            if (_itFlips)
            {
                float auxDirection = _moveDirection;
                if (_flipsInOpositeDirection)
                    auxDirection *= -1;
                if (auxDirection < 0 && _isFacingRight)
                {
                    OnFlip?.Invoke();
                }
                if (auxDirection > 0 && !_isFacingRight)
                {
                    OnFlip?.Invoke();
                }
            }
        }
        public void Flip()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        public void EnableMove()
        {
            _canMove = true;
        }

        public void DisableMove()
        {
            _canMove = false;
        }
    }
}