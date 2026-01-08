using UnityEngine;
using static ScriptableObjects.SerializableDictionaryLite.Example.DataBaseExample;

public abstract class SpaceShooterWeapon : MonoBehaviour
{
    protected SpaceShooterEnemy enemy;
    protected Transform player;
    protected float lastShot;
    public int index;

    const float MIN_X = -1.5f;
    const float MAX_X = 0.5f;
    const float MIN_Y = -1.0f;
    const float MAX_Y = 1.0f;

    public virtual void Init(SpaceShooterEnemy e)
    {
        enemy = e;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Tick()
    {
        if (Time.time - lastShot >= 3.75f-enemy.attackSpeed && IsEnemyInsidePlayArea() && player != null)
        {
            Shoot();
            lastShot = Time.time;
        }
    }

    protected bool IsEnemyInsidePlayArea()
    {
        Vector3 pos = this.transform.position;

        return pos.x >= MIN_X && pos.x <= MAX_X &&
               pos.y >= MIN_Y && pos.y <= MAX_Y;
    }

    protected abstract void Shoot();
}
