using System.Collections;
using System.Collections.Generic;
using PlatformGame.Weapons.Projectiles;
using ScriptableObjects;
using UnityEngine;

namespace PlatformGame.Weapons
{
    public class RangedWeaponController : WeaponController
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float launchPower = 5f;

        public override void LoadWeapon(EnemySO enemySo)
        {
            base.LoadWeapon(enemySo);
            launchPower = enemySo.projectileSpeed;
        }

        public override IEnumerator UseWeapon()
        {
            InstantiateProjectile();
            yield return null;
        }

        private void InstantiateProjectile()
        {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<ProjectileController>().InitialiseProjectile(weaponDamage, launchPower);
        }
    }
}
