using UnityEngine;

namespace Game.LevelManager
{
    public class WallBhv : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            //Debug.Log("CollidedWithSomething");
            if (collider.CompareTag("Bullet") || collider.CompareTag("EnemyBullet"))
            {
                //Debug.Log("CollidedWithBullet");
                collider.GetComponent<ProjectileController>().DestroyBullet();
            }
        }
    }
}