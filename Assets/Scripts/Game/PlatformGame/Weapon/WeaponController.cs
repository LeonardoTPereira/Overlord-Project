using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace PlatformGame.Weapons
{
    public abstract class WeaponController : MonoBehaviour
    {
        public static event Action<int> PlayerHitEvent;
        
        [SerializeField] protected int weaponDamage = 1;

        public virtual void LoadWeapon(EnemySO enemySo){
            weaponDamage = enemySo.damage;
        }

        public abstract IEnumerator UseWeapon();
        
        public static void PlayerHit(int damage)
        {
            PlayerHitEvent?.Invoke(damage);
        }

    }
}