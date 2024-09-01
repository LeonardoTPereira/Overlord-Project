using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace PlatformGame.Weapons
{
    public abstract class MeleeWeaponController : WeaponController
    {
        [SerializeField] protected float attackDuration;
        private Collider2D _attackCollider;

        protected virtual void Awake()
        {
            _attackCollider = GetComponent<Collider2D>();
            _attackCollider.enabled = false;
        }

        public override IEnumerator UseWeapon()
        {
            _attackCollider.enabled = true;
            OnUsing();
            yield return StartCoroutine(Cooldown());
            _attackCollider.enabled = false;
        }

        protected abstract void OnUsing();

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(attackDuration);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (CompareTag("PlayerMelee"))
            {
                if (!col.gameObject.CompareTag("Enemy")) return;
                col.gameObject.GetComponent<HealthController>().TakeDamage(weaponDamage);
                AttackHit();
            }
            else if (CompareTag("EnemyMelee"))
            {
                if (!col.gameObject.CompareTag("Player")) return;
                col.gameObject.GetComponent<HealthController>().TakeDamage(weaponDamage);
                AttackHit();
            }
        }

        private void AttackHit()
        {
            //do something
        }
    }
}