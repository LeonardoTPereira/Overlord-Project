using UnityEngine;

public abstract class SpaceShooterMovement: MonoBehaviour
{
    protected SpaceShooterEnemy enemy;
    protected Transform player;
    public int index;

    public virtual void Init(SpaceShooterEnemy e)
    {
        enemy = e;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public abstract void Tick();

    protected bool IsVisible()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > -0.1f && v.x < 1.1f && v.y > -0.1f && v.y < 1.1f;
    }
}
