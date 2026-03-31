using UnityEngine;

public class MoveHorizontalOscillate : SpaceShooterMovement
{
    private float time;
    private float startX;

    const float oscillationSpeed = 3f;
    const float minX = -1.5f;
    const float maxX = 0.5f;

    public override void Init(SpaceShooterEnemy e)
    {
        base.Init(e);
        startX = transform.position.x;
    }

    public override void Tick()
    {
        time += Time.deltaTime;

        float amplitude = (maxX - minX) * 0.5f;
        float centerX = (maxX + minX) * 0.5f;

        float x =
            centerX +
            Mathf.Sin(time * oscillationSpeed * enemy.moveSpeed) * amplitude;

        transform.position = new Vector3(
            x,
            transform.position.y,
            transform.position.z
        );
    }
}
