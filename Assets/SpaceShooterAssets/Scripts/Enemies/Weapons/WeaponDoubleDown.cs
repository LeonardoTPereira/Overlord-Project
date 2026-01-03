using UnityEngine;

public class WeaponDoubleDown : WeaponStraightDown
{
    protected override void Shoot()
    {
        Spawn(Vector2.down + Vector2.left * 0.2f);
        Spawn(Vector2.down + Vector2.right * 0.2f);
    }
}