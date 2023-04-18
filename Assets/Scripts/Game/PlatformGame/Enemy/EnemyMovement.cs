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
        public LayerMask _groundLayerMask;
                
        private float _moveDirection;
        private bool _canMove;

        private GameObject _player;

        private float _speed;

        [HideInInspector] public MovementManager moveManager;
        protected bool _itFlips = true;
        protected bool _flipsInOpositeDirection = false;

        public void LoadMovement(EnemySO enemySo)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
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

        protected virtual void GenerateMovementComponent(Enums.MovementEnum moveEnum)
        {
            switch (moveEnum)
            {
                case Enums.MovementEnum.None:
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(NoMovementManager)) as NoMovementManager;
                    break;
                case Enums.MovementEnum.Random: // This trigger basic horizontal movement, from one right to the left
                    moveManager = gameObject.AddComponent(typeof(PatrolMovementManager)) as PatrolMovementManager;
                    break;
                case Enums.MovementEnum.Random1D:
                    break;
                case Enums.MovementEnum.Flee1D:
                    _itFlips = true;
                    _flipsInOpositeDirection = true;
                    moveManager = gameObject.AddComponent(typeof(Flee1DMovementManager)) as Flee1DMovementManager;
                    break;
                case Enums.MovementEnum.Flee:
                    moveManager = gameObject.AddComponent(typeof(FleeMovementManager)) as FleeMovementManager;
                    _itFlips = true;
                    _flipsInOpositeDirection = true;
                    break;
                case Enums.MovementEnum.Follow1D:
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(Follow1DMovementManager)) as Follow1DMovementManager;
                    break;
                case Enums.MovementEnum.Follow:
                    _itFlips = true;
                    moveManager = gameObject.AddComponent(typeof(FollowMovementManager)) as FollowMovementManager;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }

        public void UpdateMovement()
        {
            UpdateDirection();
            moveManager.Move(_moveDirection, _speed, _canMove, _groundLayerMask);
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