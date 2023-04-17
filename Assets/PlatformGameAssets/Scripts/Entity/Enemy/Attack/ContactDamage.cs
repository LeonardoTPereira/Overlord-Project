using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class ContactDamage : MonoBehaviour
    {
        [SerializeField] private int contactDamage = 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                //col.gameObject.GetComponent<Player.Health>().TakeDamage(contactDamage);
            }
        }
    }

    
}