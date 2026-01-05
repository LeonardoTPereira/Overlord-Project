using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimPlayer : SpaceShooterWeapon
{
    public GameObject projectile;

    public override void Init(SpaceShooterEnemy e)
    {
        base.Init(e);
        projectile = Resources.Load<GameObject>("SpaceShooterPrefabs/AimBullet");

        if (projectile == null)
        {
            Debug.LogError($"Prefab não encontrado: {projectile}");
            return;
        }
    }

    protected override void Shoot()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        Instantiate(projectile, transform.position, Quaternion.identity)
            .GetComponent<EnemyProjectile>()
            .Init(dir, enemy.projectileSpeed, enemy.bulletRange);
    }
}