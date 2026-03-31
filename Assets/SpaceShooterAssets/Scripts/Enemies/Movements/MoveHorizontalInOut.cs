using UnityEngine;

public class MoveHorizontalInOut : SpaceShooterMovement
{
    private int dir = 1;

    public override void Tick()
    {
        transform.position += Vector3.right * dir * enemy.moveSpeed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x) > 3f)
            dir *= -1;
    }
}