using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Weapons.Projectiles
{
    public class ArrowProjectileController : ProjectileController
    {
        [SerializeField] private float arrowRightImpulseScale = 8f;
        [SerializeField] private float arrowUpImpulseScale = 1f;
        protected override IEnumerator Move()
        {
            var projectileTransform = transform;
            var force = (projectileTransform.up*arrowUpImpulseScale + projectileTransform.right*arrowRightImpulseScale).normalized;
            force = force * LaunchPower;
            ProjectileRigidbody2D.AddForce(force, ForceMode2D.Impulse);
            yield return null;
        }
        

    }
    
}
