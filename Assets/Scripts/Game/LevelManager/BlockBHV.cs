using UnityEngine;

namespace Game.LevelManager
{
    public class BlockBHV : TileBHV
    {
        // Use this for initialization
        void Start()
        {
            GetComponent<Collider2D>().enabled = true; // ativa o colisor
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("CollidedWithSomething");
            if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("EnemyBullet"))
            {
                Debug.Log("CollidedWithBullet");
                collision.gameObject.GetComponent<ProjectileController>().DestroyBullet();
            }
        }

    }
}