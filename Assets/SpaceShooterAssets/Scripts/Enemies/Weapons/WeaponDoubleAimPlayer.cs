using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDoubleAimPlayer : SpaceShooterWeapon
{
    public GameObject projectile;
    public float spreadAngle = 8f; // graus

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
        Vector2 baseDir = (player.position - transform.position).normalized;

        Vector2 dir1 = Rotate(baseDir, spreadAngle * 0.5f);
        Vector2 dir2 = Rotate(baseDir, -spreadAngle * 0.5f);

        Spawn(dir1);
        Spawn(dir2);
    }

    void Spawn(Vector2 dir)
    {
        Instantiate(projectile, transform.position, Quaternion.identity)
            .GetComponent<EnemyProjectile>()
            .Init(dir, enemy.projectileSpeed, enemy.projectileSpeed);
    }

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }
}
