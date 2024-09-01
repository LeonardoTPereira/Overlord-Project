using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class ProjectileController : MonoBehaviour
    {

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(30);
        
            DestroyProjectile();                
        }
    
        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                /*var health = col.gameObject.GetComponent<Entity.Health>();
                health.TakeDamage(1);
                DestroyProjectile();*/
            }
        
            else if (col.gameObject.CompareTag("Block"))
            {
                DestroyProjectile();
            }
        }
    }

    
}
