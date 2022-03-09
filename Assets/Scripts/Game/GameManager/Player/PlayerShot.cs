using System;
using System.Collections;
using Game.Events;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameManager.Player
{
    public class PlayerShot : PlayerInput
    {
        [SerializeField]
        protected float shootSpeed, coolDownTime, atkSpeed;
        private bool _canShoot;
        private bool _isHoldingShoot;
        private int _currentProjectile;
        [SerializeField]
        protected GameObject bulletSpawn, bulletPrefab;
        [SerializeField]
        private ProjectileTypeSO projectileType;
        private Rigidbody2D _rigidbody2D;
        private static readonly int LastDirX = Animator.StringToHash("LastDirX");
        private static readonly int LastDirY = Animator.StringToHash("LastDirY");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private void Awake()
        {
            _currentProjectile = 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            WeaponLoaderBHV.LoadWeaponButtonEventHandler += SetProjectileSO;
        }
        
        protected override void OnDisable()
        {
            base.OnEnable();
            WeaponLoaderBHV.LoadWeaponButtonEventHandler -= SetProjectileSO;
        }

        protected override void Start()
        {
            base.Start();         
            projectileType = GameManagerSingleton.Instance.projectileSet.Items[_currentProjectile];
            SetProjectileSO(this, new LoadWeaponButtonEventArgs(GameManagerSingleton.Instance.projectileType));
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
                int rotatedAngle;
                Vector2 shootForce;
                if (shotDirection.x > 0.01f)
                {
                    rotatedAngle = 0;
                    shootForce = new Vector2(shootSpeed, 0f);
                }
                else if (shotDirection.x < -0.01f)
                {
                    rotatedAngle = 180;
                    shootForce = new Vector2(-shootSpeed, 0f);
                }
                else if (shotDirection.y > 0.01f)
                {
                    rotatedAngle = 90;
                    shootForce = new Vector2(0f, shootSpeed);
                }
                else
                {
                    rotatedAngle = 270;
                    shootForce = new Vector2(0f, -shootSpeed);
                }
                UpdateShotAnimation(shotDirection);
                bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, rotatedAngle);
                var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                var bulletController = bullet.GetComponent<ProjectileController>();
                bulletController.ProjectileSO = projectileType;
                bulletController.Shoot(shootForce  + _rigidbody2D.velocity.normalized);
                var position = transform.position;
                bulletSpawn.transform.RotateAround(position, Vector3.forward, rotatedAngle + 90);
                bullet.GetComponent<Rigidbody2D>().AddForce(shootForce + _rigidbody2D.velocity.normalized, ForceMode2D.Impulse);
                bulletSpawn.transform.RotateAround(position, Vector3.forward, -rotatedAngle);
                coolDownTime = 1.0f / atkSpeed;
                StartCoroutine(CountCooldown(coolDownTime));
            }
        }

        public void ChangeWeapon(InputAction.CallbackContext context)
        {
            NextProjectileSO();
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
        
        private void FixedUpdate()
        {
            if (coolDownTime > 0.0f)
            {
                coolDownTime -= Time.fixedDeltaTime;
            }
        }

        protected override void StartInput(object sender, EventArgs eventArgs)
        {
            _canShoot = true;
        }

        protected override void StopInput(object sender, EventArgs eventArgs)
        {
            _canShoot = false;
        }
        
        private void SetProjectileSO(object sender, LoadWeaponButtonEventArgs eventArgs)
        {
            projectileType = eventArgs.ProjectileSO;
            bulletPrefab = projectileType.projectilePrefab;
            bulletPrefab.GetComponent<ProjectileController>().ProjectileSO = projectileType;
            atkSpeed = projectileType.atkSpeed;
            bulletPrefab.GetComponent<SpriteRenderer>().color = eventArgs.ProjectileSO.color;
        }
        
        private void NextProjectileSO()
        {
            _currentProjectile = (_currentProjectile + 1) % GameManagerSingleton.Instance.projectileSet.Items.Count;
            projectileType = GameManagerSingleton.Instance.projectileSet.Items[_currentProjectile];
            SetProjectileSO(this, new LoadWeaponButtonEventArgs(projectileType));
        }
    }
}