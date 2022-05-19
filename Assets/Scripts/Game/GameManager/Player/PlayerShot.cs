using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameManager.Player
{
    public class PlayerShot : PlayerInput
    {
        
        private struct BulletForceAndRotation
        {
            public Vector2 Force;
            public int Rotation;
        }
        [SerializeField]
        protected float shootSpeed, atkSpeed;
        private bool _canShoot;
        private bool _isHoldingShoot;
        [SerializeField] private GameObject bulletSpawn;
        [SerializeField] private GameObject bulletPrefab;
        [field: SerializeField] public ProjectileTypeSO projectileType { get; set; }
        [SerializeField] private ProjectileTypeRuntimeSetSO projectilesAvailable;
        private Rigidbody2D _rigidbody2D;
        private static readonly int LastDirX = Animator.StringToHash("LastDirX");
        private static readonly int LastDirY = Animator.StringToHash("LastDirY");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        protected override void Start()
        {
            base.Start();
            SetProjectileSo();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isHoldingShoot = true;
                PlayerAnimator.SetBool(IsShooting, true);
            }
            else if (context.canceled)
            {
                _isHoldingShoot = false;
                PlayerAnimator.SetBool(IsShooting, false);
            }
            var inputX = context.ReadValue<Vector2>().x;
            var inputY = context.ReadValue<Vector2>().y;
            var shotDirection = new Vector2(inputX, inputY);
            shotDirection.Normalize();
            if (shotDirection.magnitude > 0.01f)
                StartCoroutine(ShootBullet(shotDirection));
        }

        private IEnumerator ShootBullet(Vector2 shotDirection)
        {
            while (_isHoldingShoot)
            {
                yield return null;
                if (!_canShoot) continue;
                var bulletForceAndRotation = GetBulletForceAndRotation(shotDirection);
                UpdateShotAnimation(shotDirection);
                RotateSpawnPoint(bulletForceAndRotation);
                SpawnAndShootBullet(bulletForceAndRotation);
                var coolDownTime = GetCooldown();
                StartCoroutine(CountCooldown(coolDownTime));
            }
        }

        private float GetCooldown()
        {
            return 1.0f / atkSpeed;
        }

        private void SpawnAndShootBullet(BulletForceAndRotation bulletForceAndRotation)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            var bulletController = bullet.GetComponent<ProjectileController>();
            bulletController.ProjectileSo = projectileType;
            bulletController.Shoot(bulletForceAndRotation.Force + _rigidbody2D.velocity.normalized);
        }

        private void RotateSpawnPoint(BulletForceAndRotation bulletForceAndRotation)
        {
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, bulletForceAndRotation.Rotation);
        }

        private BulletForceAndRotation GetBulletForceAndRotation(Vector2 shotDirection)
        {
            BulletForceAndRotation bulletForceAndRotation;
            if (shotDirection.x > 0.01f)
            {
                bulletForceAndRotation.Rotation = 0;
                bulletForceAndRotation.Force = new Vector2(shootSpeed, 0f);
            }
            else if (shotDirection.x < -0.01f)
            {
                bulletForceAndRotation.Rotation = 180;
                bulletForceAndRotation.Force = new Vector2(-shootSpeed, 0f);
            }
            else if (shotDirection.y > 0.01f)
            {
                bulletForceAndRotation.Rotation = 90;
                bulletForceAndRotation.Force = new Vector2(0f, shootSpeed);
            }
            else
            {
                bulletForceAndRotation.Rotation = 270;
                bulletForceAndRotation.Force = new Vector2(0f, -shootSpeed);
            }

            return bulletForceAndRotation;
        }

        public void ChangeWeapon(InputAction.CallbackContext context)
        {
            NextProjectileSo();
        }

        private IEnumerator CountCooldown(float bulletCooldown)
        {
            _canShoot = false;
            yield return new WaitForSeconds(bulletCooldown);
            _canShoot = true;
        }
        
        private void UpdateShotAnimation(Vector2 shotDirection)
        {
            PlayerAnimator.SetFloat(LastDirX, shotDirection.x);
            PlayerAnimator.SetFloat(LastDirY, shotDirection.y);
        }

        protected override void StartInput(object sender, EventArgs eventArgs)
        {
            _canShoot = true;
        }

        protected override void StopInput(object sender, EventArgs eventArgs)
        {
            _canShoot = false;
        }
        
        private void SetProjectileSo()
        {
            bulletPrefab = projectileType.projectilePrefab;
            bulletPrefab.GetComponent<ProjectileController>().ProjectileSo = projectileType;
            atkSpeed = projectileType.atkSpeed;
            bulletPrefab.GetComponent<SpriteRenderer>().color = projectileType.color;
        }
        
        private void NextProjectileSo()
        {
            var currentIndex = projectilesAvailable.Items.IndexOf(projectileType);
            var nextIndex = (currentIndex + 1) % projectilesAvailable.Items.Count;
            projectileType.Copy(projectilesAvailable.Items[nextIndex]);
            SetProjectileSo();
        }
    }
}