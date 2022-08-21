using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlatformGame.Player;
using ScriptableObjects;

namespace PlatformGame.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        public event Action OnFlip;
        
        private float _moveDirection;
        private bool _isFacingRight;
        private Rigidbody2D _rb;
        private bool _canMove;
        private EnemyAnimation _enemyAnimation;

        private GameObject _player;
        private MovementType _movementType;

        private float _speed;

        public void LoadMovement(EnemySO enemySo)
        {
            _movementType = enemySo.movement.movementType;
            _speed = enemySo.movementSpeed;
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
            GetAllComponents();
            InitialiseVariables();
        }
        
        private void GetAllComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
            _enemyAnimation = GetComponent<EnemyAnimation>();
            _player = GameObject.FindGameObjectWithTag("Player");

        }

        private void InitialiseVariables()
        {
            _canMove = true;
            _moveDirection = 1;
            _isFacingRight = true;
            _movementType = NoMovement;
        }

        private Vector2 NoMovement(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false)
        {
            return Vector2.zero;
        }


        private void Update()
        {
            if (!CanMove()) return;
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            var directionMask = new Vector2();
            _moveDirection = _movementType(_player.transform.position, transform.position, ref directionMask, false).x;
        }

        private bool CanMove()
        {
            return _canMove;
        }

        public void UpdateMovement()
        {
            if (CanMove())
            {
                Move();
            }
            AnimateMove();
        }

        private void Move()
        {
            VerifyOrientationAndFlip();   
            MakeTranslation();
        }

        private void VerifyOrientationAndFlip()
        {
            if (_moveDirection < 0 && _isFacingRight)
            {
                OnFlip?.Invoke();
            }
            if (_moveDirection > 0 && !_isFacingRight)
            {
                OnFlip?.Invoke();
            }
        }
        private void MakeTranslation()
        {
            _rb.transform.position += Vector3.right*_moveDirection*Time.fixedDeltaTime*_speed;
        }
        
        private void AnimateMove()
        {
            _enemyAnimation.AnimateMove(false , Mathf.Abs(_moveDirection), _canMove);
        }

        
        private void Flip()
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