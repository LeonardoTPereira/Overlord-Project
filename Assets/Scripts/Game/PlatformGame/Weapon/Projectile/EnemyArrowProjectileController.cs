using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Util;

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

            // Projectile speed is setted from 1f to 4f in SearchSpace
            LaunchPower = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(LaunchPower, _minimumForce, _maximumForce, 1f, 4f);

            force = force * LaunchPower;
            ProjectileRigidbody2D.AddForce(force, ForceMode2D.Impulse);
            yield return null;
        }
    }    
}
