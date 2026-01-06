using UnityEngine;
using static ScriptableObjects.SerializableDictionaryLite.Example.DataBaseExample;

public class WeaponStraightDown : SpaceShooterWeapon
{
    public GameObject projectile;

    public override void Init(SpaceShooterEnemy e)
    {
        base.Init(e);
        projectile = Resources.Load<GameObject>("SpaceShooterPrefabs/StraightBullet");

        if (projectile == null)
        {
            Debug.LogError($"Prefab não encontrado: {projectile}");
            return;
        }
    }

    protected override void Shoot()
    {
        Spawn(Vector2.down);
    }

    protected void Spawn(Vector2 dir)
    {
        Instantiate(projectile, transform.position, Quaternion.identity)
            .GetComponent<EnemyProjectile>()
            .Init(dir, enemy.projectileSpeed, enemy.projectileSpeed);
    }
}

