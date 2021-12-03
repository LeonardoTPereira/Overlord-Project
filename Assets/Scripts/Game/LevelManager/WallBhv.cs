﻿using UnityEngine;

namespace Game.LevelManager
{
    public class WallBhv : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Bullet") || collider.CompareTag("EnemyBullet"))
            {
                collider.GetComponent<ProjectileController>().DestroyBullet();
            }
        }
    }
}