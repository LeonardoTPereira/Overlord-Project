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


        private float _speed;

        protected MovementManager _moveManager;
        private bool _itFlips = true;

        [Header("Test attributes (only used if isThisATest is marked)")]
        [SerializeField] private bool _isThisATest = false;
        [SerializeField] private Enums.MovementEnum _movementEnum;

        public void LoadMovement(EnemySO enemySo)
        {
            // From 0.8f to 3.2f in SearchSpace
            _speed = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.movementSpeed, _minimumSpeed, _maximumSpeed, .8f, 3.2f);

            if (!_isThisATest)
                _movementEnum = enemySo.movement.enemyMovementIndex;

            // This movement doesn't flip to player direction
            if (_movementEnum == Enums.MovementEnum.Follow1D)
                _itFlips = false;

            GenerateMovementComponent(_movementEnum);
        }
        
        
        private void OnEnable()
        {
            OnFlip += Flip;
        }

        private void OnDisable()
        {
            OnFlip -= Flip;
        }
        
        private void Awake()
        {
            InitialiseVariables();
        }
        
        private void InitialiseVariables()
        {
            _canMove = true;
            _moveDirection = 1;
            _isFacingRight = true;
        }

        protected virtual void GenerateMovementComponent(Enums.MovementEnum moveEnum)
        {
            _moveManager = null;
            switch (moveEnum)
            {
                case Enums.MovementEnum.None:
                    break;
                case Enums.MovementEnum.Random:
                    break;
                case Enums.MovementEnum.Random1D:
                    break;
                case Enums.MovementEnum.Flee1D:
                    break;
                case Enums.MovementEnum.Flee:
                    break;
                case Enums.MovementEnum.Follow1D:
                    break;
                case Enums.MovementEnum.Follow:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }

        private Vector2 NoMovement(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return Vector2.zero;
        }

        public void UpdateMovement()
        {
            _moveManager.Move(_speed, _canMove);
            VerifyOrientationAndFlip();  
        }

        private void VerifyOrientationAndFlip()
        {
            if (_moveDirection < 0 && _isFacingRight && _itFlips)
            {
                OnFlip?.Invoke();
            }
            if (_moveDirection > 0 && !_isFacingRight && _itFlips)
            {
                OnFlip?.Invoke();
            }
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