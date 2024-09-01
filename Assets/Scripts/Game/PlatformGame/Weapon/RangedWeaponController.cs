using System.Collections;
using System.Collections.Generic;
using PlatformGame.Weapons.Projectiles;
using ScriptableObjects;
using UnityEngine;
using PlatformGame.Util;

namespace PlatformGame.Weapons
{
    public class RangedWeaponController : WeaponController
    {
        [SerializeField] private float _minimumAttackSpeed = 0.75f;
        [SerializeField] private float _maximumAttackSpeed = 4f;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float launchPower = 5f;

        public override void LoadWeapon(EnemySO enemySo)
        {
            base.LoadWeapon(enemySo);
            launchPower = CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform(enemySo.projectileSpeed, _minimumAttackSpeed, _maximumAttackSpeed, 0.75f, 4f);
            Debug.Log("enemySo: " + enemySo.projectileSpeed + "lauchPower: " + launchPower);
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
