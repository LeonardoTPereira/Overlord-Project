using System;
using Game.Audio;
using Game.GameManager.Player;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.GameManager
{
    public class ProjectileController : MonoBehaviour, ISoundEmitter
    {
        private Rigidbody2D rb;
        private bool IsSin { get; set; }
        private int EnemyThatShot { get; set; }

        public static event EventHandler EnemyHitEventHandler;
        public static event EventHandler PlayerHitEventHandler;

        private Vector2 pos, moveDir;
        private float MoveSpeed { get; set; }

        [SerializeField]
        private float frequency, magnitude;
        [SerializeField]
        private int damage;
        [field: SerializeField] public ProjectileTypeSO ProjectileSo { get; set; }

        private Rigidbody2D bulletRigidBody;
    
        // Use this for initialization
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            IsSin = false;
        }

        private void Start()
        {
            if (ProjectileSo == null)
                Debug.LogError("NO PROJECTILE SO!!!");
            MoveSpeed = ProjectileSo.moveSpeed;
            damage = ProjectileSo.damage;
            bulletRigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += PlayerHasDied;
        }

        public void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
        }

        private void PlayerHasDied(object sender, EventArgs eventArgs)
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!IsSin) return;
            pos += moveDir * (Time.deltaTime * MoveSpeed);
            transform.position = pos + Vector2.Perpendicular(moveDir).normalized * (Mathf.Sin(Time.time * frequency) * magnitude);
        }

        public void DestroyBullet()
        {
            ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.BulletHit, 1));
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }

        private void OnEnemyHit()
        {
            EnemyHitEventHandler?.Invoke(null, EventArgs.Empty);
        }

        private void OnPlayerHit()
        {
            PlayerHitEventHandler?.Invoke(null, EventArgs.Empty);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var collisionDirection = Vector3.Normalize(collision.gameObject.transform.position - gameObject.transform.position);
            if (CompareTag("EnemyBullet"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    OnPlayerHit();
                    collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage, collisionDirection, EnemyThatShot);
                    DestroyBullet();
                }
            }
            else if (CompareTag("Bullet"))
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    OnEnemyHit();
                    collision.gameObject.GetComponent<EnemyController>().ApplyDamageEffects(collisionDirection);
                    collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage, collisionDirection);
                    DestroyBullet();
                }
                if (collision.gameObject.CompareTag("Shield"))
                {
                    DestroyBullet();
                }
            }
            if (collision.gameObject.CompareTag("Block"))
            {
                DestroyBullet();
            }
        }

        public void SetEnemyThatShot(int index)
        {
            EnemyThatShot = index;
        }

        public void Shoot(Vector2 facingDirection)
        {
            Enums.PlayerProjectileEnum projEnum = ProjectileSo.projectileBehaviorIndex;
            switch (projEnum)
            {
                case Enums.PlayerProjectileEnum.Straight:
                    StraightShot(facingDirection);
                    break;
                case Enums.PlayerProjectileEnum.Sin:
                    SinShot(facingDirection);
                    break;
                case Enums.PlayerProjectileEnum.Triple:
                    TripleShot(facingDirection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(projEnum.ToString(), "Projectile index does not exist in the PlayerProjectileEnum");
            } 
        }

        private void StraightShot(Vector2 facingDirection)
        {
            rb.AddForce(facingDirection, ForceMode2D.Impulse);
        }
        private void SinShot(Vector2 facingDirection)
        {
            pos = transform.position;
            moveDir = facingDirection.normalized;
            IsSin = true;
        }

        private void TripleShot(Vector2 facingDirection)
        {
            Vector2 right = Quaternion.Euler(0, 0, 30) * facingDirection;
            Vector2 left = Quaternion.Euler(0, 0, -30) * facingDirection;
            var bulletTransform = transform;
            var position = bulletTransform.position;
            var rotation = bulletTransform.rotation;
            var rightBullet = Instantiate(this, position, rotation);
            var leftBullet = Instantiate(this, position, rotation);
            rightBullet.ProjectileSo = ProjectileSo;
            leftBullet.ProjectileSo = ProjectileSo;
            rightBullet.bulletRigidBody.AddForce(right, ForceMode2D.Impulse);
            leftBullet.bulletRigidBody.AddForce(left, ForceMode2D.Impulse);
            rb.AddForce(facingDirection, ForceMode2D.Impulse);
        }
    }
}
