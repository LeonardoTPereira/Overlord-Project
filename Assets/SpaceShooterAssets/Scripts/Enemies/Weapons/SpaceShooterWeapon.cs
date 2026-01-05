using UnityEngine;

public abstract class SpaceShooterWeapon : MonoBehaviour
{
    protected SpaceShooterEnemy enemy;
    protected Transform player;
    protected float lastShot;
    public int index;

    public virtual void Init(SpaceShooterEnemy e)
    {
        enemy = e;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Tick()
    {
        if (Time.time - lastShot >= 3.75f-enemy.attackSpeed)
        {
            Shoot();
            lastShot = Time.time;
        }
    }

    protected abstract void Shoot();
}
