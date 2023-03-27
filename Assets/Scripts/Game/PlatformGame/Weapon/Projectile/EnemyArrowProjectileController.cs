using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Weapons.Projectiles
{
    public class EnemyArrowProjectileController : ProjectileController
    {
        [SerializeField] private float _minimumForce = 15f;
        [SerializeField] private float _maximumForce = 60f;

        private float arrowRightImpulseScale = 15f;
        private float arrowUpImpulseScale = 1f;

        protected override IEnumerator Move()
        {
            var projectileTransform = transform;
            var force = (projectileTransform.up*arrowUpImpulseScale + projectileTransform.right*arrowRightImpulseScale).normalized;

            CalculateEnemyLaunchPower();

            force = force * LaunchPower;
            ProjectileRigidbody2D.AddForce(force, ForceMode2D.Impulse);
            yield return null;
        }

        // Since the values of projectile speed is setted to the top-down game (from 1f to 4f in SearchSpace)
        // for this enemy, we need to calculate using new values for platform game
        private void CalculateEnemyLaunchPower()
        {
            LaunchPower = _minimumForce + (_maximumForce - _minimumForce) *(LaunchPower - 1f) / (4f - 1f);
        }

    }
    
}
