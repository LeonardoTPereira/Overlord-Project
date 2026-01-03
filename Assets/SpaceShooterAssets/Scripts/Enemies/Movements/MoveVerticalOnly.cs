using UnityEngine;

public class MoveVerticalOnly : SpaceShooterMovement
{
    public override void Tick()
    {
        transform.position += Vector3.down * enemy.moveSpeed * Time.deltaTime;
    }
}