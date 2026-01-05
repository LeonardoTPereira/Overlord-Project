using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRadial6 : SpaceShooterWeapon
{
    public GameObject projectile;

    public override void Init(SpaceShooterEnemy e)
    {
        base.Init(e);
        projectile = Resources.Load<GameObject>("SpaceShooterPrefabs/RadialBullet");

        if (projectile == null)
        {
            Debug.LogError($"Prefab não encontrado: {projectile}");
            return;
        }
    }

    protected override void Shoot()
    {
        int bullets = 6;
        float step = 360f / bullets;

        for (int i = 0; i < bullets; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Instantiate(projectile, transform.position, Quaternion.identity)
                .GetComponent<EnemyProjectile>()
                .Init(dir, enemy.projectileSpeed, enemy.bulletRange);
        }
    }

    /*
    protected override void Shoot()
{
    angleOffset += 15f;

    for (int i = 0; i < 6; i++)
    {
        float angle = (angleOffset + i * 60) * Mathf.Deg2Rad;
        Vector2 dir = new(Mathf.Cos(angle), Mathf.Sin(angle));
        Spawn(dir);
    }
}
    */
}
