using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChaseAndExit : SpaceShooterMovement
{
    public override void Tick()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * enemy.moveSpeed * Time.deltaTime);

        if (!IsVisible())
            Destroy(gameObject);
    }

    bool IsVisible()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > -0.1f && v.x < 1.1f && v.y > -0.1f;
    }
}
