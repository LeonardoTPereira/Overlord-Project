using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChaseAndExit : SpaceShooterMovement
{
    private Vector2 velocity;
    private bool exiting;

    [SerializeField] private float steerStrength = 3f;

    public override void Init(SpaceShooterEnemy e)
    {
        base.Init(e);

        // Initial downward movement (Touhou-like)
        velocity = Vector2.down * enemy.moveSpeed;
    }

    public override void Tick()
    {
        if (!exiting)
        {
            SteerTowardsPlayer();

            // Check if enemy passed the player (Y axis shooter)
            if (transform.position.y < player.position.y)
            {
                exiting = true;
            }
        }

        transform.position += (Vector3)(velocity*2 * Time.deltaTime);

        if (!IsVisible())
            Destroy(gameObject);
    }

    private void SteerTowardsPlayer()
    {
        Vector2 desiredDir =
            (player.position - transform.position).normalized * enemy.moveSpeed;

        // Smooth steering instead of snapping
        velocity = Vector2.Lerp(
            velocity,
            desiredDir,
            steerStrength * Time.deltaTime
        );
    }

    private bool IsVisible()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > -0.1f && v.x < 1.1f && v.y > -0.1f && v.y < 1.1f;
    }
}

