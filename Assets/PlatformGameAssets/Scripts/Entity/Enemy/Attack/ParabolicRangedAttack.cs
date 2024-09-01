using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Enemy
{
    
    public class ParabolicRangedAttack : Attack
{
        [SerializeField] private float gravity;
        private Transform target;

    protected override void Initialize()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    protected override void Shoot()
    {
        var currentBullet = Instantiate(bullet, mouth.position, transform.rotation);

        var angle = GetCurrentLaunchAngle();
        var speed = GetCurrentLaunchSpeed(angle);
        
        var bulletVelocity = (new Vector2(Mathf.Cos(angle) * transform.right.x, Mathf.Sin(angle))) * speed ;
        
        currentBullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        
    }

    private float GetCurrentLaunchAngle()
    {
        var relativePosition = target.position - transform.position;
        var a = relativePosition.x;
        var b = relativePosition.y;

        if (a < 0)
            a = -a;

        var minAngle = Mathf.Atan2(b, a);

        if (minAngle >= Mathf.Deg2Rad * 80f)
            return Mathf.Deg2Rad * 90f;
        
        if (minAngle >= Mathf.Deg2Rad * 60f)
            return Mathf.Deg2Rad * 80f;
        
        if (minAngle >= Mathf.Deg2Rad * 45f)
            return Mathf.Deg2Rad * 60f;
        
        return Mathf.Deg2Rad * 45f;

    }
    
    private float GetCurrentLaunchSpeed(float angle)
    {
        var relativePosition = target.position - transform.position;
        var a = relativePosition.x;
        var b = relativePosition.y;

        if (a < 0)
            a = -a;

        if (a * Mathf.Tan(Mathf.Deg2Rad * 80f) < b)
            return 100f;    
        
        return Mathf.Sqrt(gravity * a * a / ( 2* Mathf.Pow(Mathf.Cos(angle), 2) * (a * Mathf.Tan(angle) - b)));
    }
    
}

    
}