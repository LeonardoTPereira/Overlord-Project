using System;
using System.Collections;
using System.Collections.Generic;
using PlatformGame.Player;
using ScriptableObjects;
using UnityEngine;

namespace PlatformGame.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyMovement _enemyMovement;
        private EnemyAttack _enemyAttack;
        private EnemyHealth _enemyHealth;
        private EnemyAnimation _enemyAnimation;
        private Collider2D _enemyCollider;
        private Rigidbody2D _enemyRigidBody;

        private bool _hasLoadedEnemy;
        private bool _isPlayerAlive;
        private bool _isPhysicsDeactivated; 
         
        
        private void OnEnable()
        {
            PlayerHealth.PlayerDiedEvent += PlayerHasDied;
            _enemyAttack.OnIsAttacking += DisableInput;
            _enemyAttack.OnStopAttacking += EnableInput;
        }
        
        private void OnDisable()
        {
            PlayerHealth.PlayerDiedEvent -= PlayerHasDied;
            _enemyAttack.OnIsAttacking -= DisableInput;
            _enemyAttack.OnStopAttacking -= EnableInput;
        }
        
        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyAttack = GetComponent<EnemyAttack>();
            _enemyHealth = GetComponent<EnemyHealth>();
            _enemyAnimation = GetComponent<EnemyAnimation>();
            _enemyCollider = GetComponent<Collider2D>();
            _enemyRigidBody = GetComponent<Rigidbody2D>();
            
            EnableInput();

            _hasLoadedEnemy = false;
            _isPlayerAlive = true;
            _isPhysicsDeactivated = false;
        }

        private void FixedUpdate()
        {
            if (_enemyHealth.IsDead())
            {
                DeactivatePhysics();
                return;
            }
            if (!_hasLoadedEnemy) return;
            _enemyMovement.UpdateMovement();
            if (!_isPlayerAlive) return;
            _enemyAttack.Attack();
        }

        private void DeactivatePhysics()
        {
            if (_isPhysicsDeactivated) return;
            _enemyCollider.enabled = false;
            _enemyRigidBody.isKinematic = true;
            _enemyRigidBody.velocity = Vector2.zero;
        }

        public void LoadEnemyData(EnemySO enemySo, int questId)
        {
            enemySo.movement.movementType = FollowPlayer; //temp for test
            _enemyMovement.LoadMovement(enemySo);
            _enemyAttack.LoadAttack(enemySo);
            _enemyHealth.LoadHealth(enemySo);
            _hasLoadedEnemy = true;
        }
        
        private Vector2 FollowPlayer(Vector2 playerPos, Vector2 enemyPos, ref Vector2 directionMask, bool updateMask = false) //temp for test
        {
            Vector2 direction = playerPos - enemyPos;
            return direction.normalized;
        }

        private void DisableInput()
        {
            if (!_isPlayerAlive) return;
            _enemyMovement.DisableMove();
        }
        
        private void EnableInput()
        {
            if (!_isPlayerAlive) return;
            _enemyMovement.EnableMove();
        }

        private void PlayerHasDied()
        {
            _isPlayerAlive = false;
            _enemyAnimation.AnimateVictory();
            _enemyMovement.DisableMove();
        }

    }
}