using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using PlatformGame.Util;

namespace PlatformGame.Weapons
{
    public abstract class WeaponController : MonoBehaviour
    {
        [SerializeField] float _minimumDamage = 1f;
        [SerializeField] float _maximumDamage = 4f;
        public static event Action<int> PlayerHitEvent;
        
        [SerializeField] protected int weaponDamage = 1;

        public virtual void LoadWeapon(EnemySO enemySo){
            // Strengh setted from 1 to 4 in SearchSpace.cs
            weaponDamage = Mathf.RoundToInt(CalculateValueEnemySoTopdownToPlatform.TopdownToPlatform((float)enemySo.damage, (float)_minimumDamage,(float)_maximumDamage, 1f, 4f));
        }

        public abstract IEnumerator UseWeapon();
        
        public static void PlayerHit(int damage)
        {
            PlayerHitEvent?.Invoke(damage);
        }

    }
}