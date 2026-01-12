using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTopLeftToBottomRight : SpaceShooterMovement
{
    private Vector2 dir = new Vector2(1, -1).normalized;

    public override void Tick()
    {
        transform.position += (Vector3)(dir * enemy.moveSpeed * Time.deltaTime);

        if (!IsVisible())
            Destroy(gameObject);
    }
}
