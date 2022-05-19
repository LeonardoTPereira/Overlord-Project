using Game.GameManager;
using UnityEngine;

namespace Game.LevelManager.DungeonManager
{
    public class BlockBhv : TileBhv
    {
        // Use this for initialization
        void Start()
        {
            GetComponent<Collider2D>().enabled = true; // ativa o colisor
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("EnemyBullet"))
            {
                collision.gameObject.GetComponent<ProjectileController>().DestroyBullet();
            }
        }

    }
}