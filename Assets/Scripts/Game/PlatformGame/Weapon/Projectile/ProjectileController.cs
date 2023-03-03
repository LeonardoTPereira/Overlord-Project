using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace PlatformGame.Weapons.Projectiles
{
    public abstract class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy = 10f;
        private int _projectileDamage;
        protected Rigidbody2D ProjectileRigidbody2D;
        protected float LaunchPower;
        private void Start()
        {
            ProjectileRigidbody2D = GetComponent<Rigidbody2D>();
            StartCoroutine(Move());
            StartCoroutine(CountCooldownToDestroy());
        }

        public void InitialiseProjectile(int projectileDamage, float projectileLaunchPower)
        {
            _projectileDamage = projectileDamage;
            LaunchPower = projectileLaunchPower;
        }

        protected abstract IEnumerator Move(); //Move projectile

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Floor"))
            {
                DestroyProjectile();
            }
            
            else if (CompareTag("PlayerProjectile"))
            {
                if (!col.gameObject.CompareTag("Enemy")) return;
                col.gameObject.GetComponent<HealthController>().TakeDamage(_projectileDamage);
                DestroyProjectile();
            }
            else if (CompareTag("EnemyProjectile"))
            {
                if (!col.gameObject.CompareTag("Player")) return;
                col.gameObject.GetComponent<HealthController>().TakeDamage(_projectileDamage);
                DestroyProjectile();
            }
        }

        private IEnumerator CountCooldownToDestroy()
        {
            yield return new WaitForSeconds(timeToDestroy);
            DestroyProjectile();
        }
    
        protected virtual void DestroyProjectile() //You can put an animation later
        {
            Destroy(gameObject);
        }
    }
}