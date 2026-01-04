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
        Debug.Log(player.position);
    }

    public abstract void Tick();
}
