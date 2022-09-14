using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Gameplay
{
    public abstract class HealthController : MonoBehaviour
    {
        private bool _canTakeDamage;
        private bool _isDead;
        protected int Health;
        [SerializeField] protected int maxHealth = 5;
        [SerializeField] protected float invincibilityCooldown = 0.5f;
        [SerializeField] private bool hasDeathAnimation = false;

        private void Start()
        {
            InitializeHealth();
        }

        protected virtual void InitializeHealth()
        {
            _canTakeDamage = true;
            _isDead = false;
            Health = maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            if (!_canTakeDamage) return;
            Health -= damage;
            CheckDeathAndKill();
            StartCoroutine(CountInvincibilityCooldown());
        }

        private void CheckDeathAndKill()
        {
            if (Health > 0) return;
            Kill();
        }

        private IEnumerator CountInvincibilityCooldown()
        {
            _canTakeDamage = false;
            yield return new WaitForSeconds(invincibilityCooldown);
            _canTakeDamage = true;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        protected virtual void Kill()
        {
            _isDead = true;
            if(!hasDeathAnimation)
                Destroy(gameObject);
                
        }

        public bool GetCanTakeDamage()
        {
            return _canTakeDamage;
        }

        public virtual void ApplyHeal(int heal)
        {
            Health = Health + heal;

            if (Health > maxHealth)
                Health = maxHealth;
        }
    }
}