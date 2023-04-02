using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformGame.Weapons;
using UnityEngine.InputSystem;

namespace PlatformGame.Player
{
    public class PlayerShots : MonoBehaviour
    {
        public static event Action IsShooting;
        public static event Action StopShooting;
        
        [SerializeField] private GameObject bow;
        [SerializeField] private float timeToAtack = 0.3f;
        private bool _canShoot;
        private PlayerAnimationController playerAnimation;
    
        [SerializeField] private float shootCooldown = 1f;

        private void OnEnable()
        {
            PlayerMovement.OnFlip += FlipBow;
        }
    
        private void OnDisable()
        {
            PlayerMovement.OnFlip -= FlipBow;
        }
    
        private void Awake()
        {
            _canShoot = true;
        }
    
        private void Start()
        {
            playerAnimation = GetComponent<PlayerAnimationController>();
        }
    
        private void Shoot()
        {
            if (_canShoot)
            {
                StartCoroutine(WaitShooting());
                StartCoroutine(CountCooldown());
            }
        }
        
        private IEnumerator CountCooldown()
        {
            _canShoot = false;
            yield return new WaitForSeconds(shootCooldown);
            _canShoot = true;
        }
    
        private IEnumerator WaitShooting()
        {
            playerAnimation.AnimateShoot();
            IsShooting?.Invoke();
            yield return new WaitForSeconds(timeToAtack);
            StartCoroutine(bow.GetComponent<WeaponController>().UseWeapon());
            StopShooting?.Invoke();
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            Shoot();
        }
    
        private void FlipBow()
        {
            bow.transform.Rotate(0f, 180f, 0f, Space.Self);
        }
    }
}

