using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlatformGame.Player;
using ScriptableObjects;
using UnityEngine.Serialization;
using PlatformGame.Weapons;

namespace PlatformGame.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        
        public event Action OnIsAttacking;
        public event Action OnStopAttacking;
        private bool _canAttack;
        private EnemyAnimation _enemyAnimation;
        private EnemyMovement _enemyMovement;
        private WeaponController _weaponController;
    
        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] protected GameObject weapon;
        [SerializeField] protected float timeToAtack = 0.3f; //This property depends on attack animation.
        
        public virtual void LoadAttack(EnemySO enemySo){
            _weaponController.LoadWeapon(enemySo);
            attackCooldown = 1/enemySo.attackSpeed;   
        }

        private void OnEnable()
        {
            _enemyMovement.OnFlip += FlipWeapon;
            PlayerHealth.PlayerDiedEvent += DisableAttack;
        }

        protected void OnDisable()
        {
            _enemyMovement.OnFlip -= FlipWeapon;
            PlayerHealth.PlayerDiedEvent -= EnableAttack;
        }
    
        protected void Awake()
        {
            _canAttack = true;
            GetAllComponents();
        }
        protected virtual void GetAllComponents()
        {
            _weaponController = weapon.GetComponent<WeaponController>();
            _enemyAnimation = GetComponent<EnemyAnimation>();
            _enemyMovement = GetComponent<EnemyMovement>();
        }
    
        public void Attack()
        {
            if (!_canAttack) return;
            AnimateAttack();
            OnIsAttacking?.Invoke();
            StartCoroutine(WaitAnimationToAttack());
            StartCoroutine(CountCooldown());
        }

        private void AnimateAttack()
        {
            _enemyAnimation.AnimateAttack();
        }

        private IEnumerator CountCooldown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }

        private IEnumerator WaitAnimationToAttack()
        {
            yield return new WaitForSeconds(timeToAtack);
            yield return StartCoroutine(UseWeapon());
            OnStopAttacking?.Invoke();
        }

        private IEnumerator UseWeapon()
        {
            yield return StartCoroutine(_weaponController.UseWeapon());
        }

        private void FlipWeapon()
        {
            weapon.transform.Rotate(0f, 180f, 0f, Space.Self);
        }

        private void EnableAttack()
        {
            _canAttack = true;
        }

        private void DisableAttack()
        {
            _canAttack = false;
        }
        
    }
}

