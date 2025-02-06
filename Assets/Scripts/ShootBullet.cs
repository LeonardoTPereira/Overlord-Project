using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public float range = 10f;
    public float damage = 5f;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;

    // Start is called before the first frame update
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLine.positionCount = 2; // Ensure the LineRenderer has 2 positions
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        // Update the origin and direction of the ray each time Shoot is called
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Set the initial point of the line
        gunLine.SetPosition(0, transform.position);

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // If we hit something
            gunLine.SetPosition(1, shootHit.point);
            // You could also handle applying damage here
            // e.g., shootHit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        else
        {
            // If we didn't hit anything, show the full range of the shot
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
