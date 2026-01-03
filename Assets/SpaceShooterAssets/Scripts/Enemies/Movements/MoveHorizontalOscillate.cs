using UnityEngine;

public class MoveHorizontalOscillate : SpaceShooterMovement
{
    private float time;

    public override void Tick()
    {
        time += Time.deltaTime;
        float x = Mathf.Sin(time * enemy.moveSpeed);
        transform.position += new Vector3(x, 0, 0) * Time.deltaTime;
    }
}
