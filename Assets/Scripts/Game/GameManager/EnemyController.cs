using System;
using System.ComponentModel;
using Game.EnemyGenerator;
using Game.GameManager.Player;
using Game.LevelManager;
using Game.LevelManager.DungeonManager;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.GameManager
{
    public class EnemyController : MonoBehaviour
    {
        /// This constant holds the weapon prefab name of healers
        private const string HealerPrefabName = "EnemyHealArea";
        private static readonly int HIT_ENEMY = 0;
        private static readonly int ENEMY_DEATH = 1;

        [field: SerializeField]
        public Sprite RandomMovementSprite { get; set; }
        [field: SerializeField]
        public Sprite FleeMovementSprite { get; set; }
        [field: SerializeField]
        public Sprite FollowMovementSprite { get; set; }
        [field: SerializeField]
        public Sprite NoneMovementSprite { get; set; }

        private bool isActive;
        private bool canDestroy;
        [SerializeField]
        private float restTime, activeTime, movementSpeed, attackSpeed, projectileSpeed;
        private int damage;
        [SerializeField]
        private GameObject playerObj, weaponPrefab, projectilePrefab, projectileSpawn, weaponSpawn, shieldSpawn;
        [SerializeField]
        private ParticleSystem bloodParticle;
        [SerializeField]
        private MovementTypeSO movement;
        private BehaviorType behavior;
        private ProjectileTypeSO projectileType;

        private Animator animator;
        private AudioSource[] audioSources;
        [SerializeField]
        private float walkUntil, waitUntil, coolDownTime;
        private bool isWalking, hasProjectile, isShooting;
        [SerializeField]
        private bool hasMoveDirBeenChosen, hasFixedMoveDir, dataHasBeenLoaded;
        private Color originalColor, armsColor, headColor, legsColor;
        private float lastX, lastY;
        [SerializeField]
        private Vector3 targetMoveDir;
        private RoomBhv room;
        [SerializeField]
        private int indexOnEnemyList;
        private HealthController healthController;
        private Rigidbody2D enemyRigidBody;
        private SpriteRenderer[] childrenSpriteRenderer;
        private Collider2D[] childrenCollider;
        private Collider2D enemyCollider;
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer armSpriteRenderer;
        private SpriteRenderer legSpriteRenderer;
        private SpriteRenderer headSpriteRenderer;
        private static readonly int LastDirX = Animator.StringToHash("LastDirX");
        private static readonly int LastDirY = Animator.StringToHash("LastDirY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");

        public static event EventHandler PlayerHitEventHandler;
        public static event EventHandler KillEnemyEventHandler;

        private bool _hasGotComponents;

        private void Start()
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
        }

        private void GetAllComponents()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyCollider = GetComponent<Collider2D>();
            childrenCollider = GetComponentsInChildren<Collider2D>();
            childrenSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
            armSpriteRenderer = gameObject.transform.Find("EnemyArms").GetComponent<SpriteRenderer>();
            legSpriteRenderer = gameObject.transform.Find("EnemyLegs").GetComponent<SpriteRenderer>();
            headSpriteRenderer = gameObject.transform.Find("EnemyHead").GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioSources = GetComponents<AudioSource>();
            healthController = gameObject.GetComponent<HealthController>();
            enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
            _hasGotComponents = true;
        }

        private void Awake()
        {
            isActive = true;
            canDestroy = false;
            dataHasBeenLoaded = false;
            _hasGotComponents = false;
            playerObj = Player.Player.Instance.gameObject;
        }

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += PlayerHasDied;
        }

        private void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
        }

        private void OnPlayerHit()
        {
            PlayerHitEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void ApplyDamageEffects(Vector3 impactDirection)
        {
            if (healthController.GetHealth() <= 0 || audioSources[HIT_ENEMY].isPlaying) return;
            audioSources[HIT_ENEMY].PlayOneShot(audioSources[HIT_ENEMY].clip, 1.0f);
            var mainParticle= bloodParticle.main;
            mainParticle.startSpeed = 0;
            var forceOverLifetime = bloodParticle.forceOverLifetime;
            forceOverLifetime.enabled = true;
            forceOverLifetime.x = impactDirection.x * 40;
            forceOverLifetime.y = impactDirection.y * 40;
            forceOverLifetime.z = impactDirection.z * 40;
            
            bloodParticle.Play();
        }

        private void PlayerHasDied(object sender, EventArgs eventArgs)
        {
            isActive = false;
        }

        private void Update()
        {
            if (!audioSources[ENEMY_DEATH].isPlaying && canDestroy)
            {
                Destroy(gameObject);
            }
        }
        
        private void FixedUpdate()
        {
            if (!dataHasBeenLoaded || !isActive || canDestroy) return;
            if (isWalking)
            {
                if (walkUntil > 0)
                    Walk();
                else
                {
                    walkUntil = 0.0f;
                    isWalking = false;
                    waitUntil = restTime;
                    hasMoveDirBeenChosen = false;
                }
            }
            else
            {
                if (waitUntil > 0f)
                    Wait();
                else
                {
                    waitUntil = 0;
                    isWalking = true;
                    walkUntil = activeTime;
                }
            }
            if (hasProjectile)
            {
                if (isShooting)
                {
                    Shoot();
                    isShooting = false;
                    coolDownTime = 1.0f / attackSpeed;
                }
                else
                {
                    if (coolDownTime > 0.0f)
                        WaitShotCoolDown();
                    else
                    {
                        isShooting = true;
                    }
                }
            }
        }

        private void Walk()
        {
            if (!hasMoveDirBeenChosen)
            {
                int xOffset, yOffset;
                targetMoveDir = movement.movementType(playerObj.transform.position, gameObject.transform.position);
                targetMoveDir.Normalize();
                //TODO Animate the enemy
                //UpdateAnimation(targetMoveDir);
                if (targetMoveDir.x > 0)
                    xOffset = 1;
                else if (targetMoveDir.x < 0)
                    xOffset = -1;
                else
                    xOffset = 0;
                if (targetMoveDir.y > 0)
                    yOffset = 1;
                else if (targetMoveDir.y < 0)
                    yOffset = -1;
                else
                    yOffset = 0;
                targetMoveDir = new Vector3((targetMoveDir.x + xOffset), (targetMoveDir.y + yOffset), 0f);
                if (hasFixedMoveDir)
                    hasMoveDirBeenChosen = true;
            }
            transform.position += new Vector3(targetMoveDir.x * movementSpeed * Time.fixedDeltaTime, targetMoveDir.y * movementSpeed * Time.fixedDeltaTime, 0f);
            walkUntil -= Time.deltaTime;
        }

        private void Wait()
        {
            enemyRigidBody.velocity = Vector3.zero;
            waitUntil -= Time.deltaTime;
        }

        private void WaitShotCoolDown()
        {
            coolDownTime -= Time.deltaTime;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var collisionDirection = Vector3.Normalize(gameObject.transform.position - collision.gameObject.transform.position);
            if (!collision.gameObject.CompareTag("Player")) return;
            OnPlayerHit();
            collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage, collisionDirection, indexOnEnemyList);
        }

        public void CheckDeath()
        {
            if (healthController.GetHealth() > 0f) return;
            audioSources[ENEMY_DEATH].PlayOneShot(audioSources[ENEMY_DEATH].clip, 1.0f);
            canDestroy = true;
            enemyCollider.enabled = false;
            spriteRenderer.enabled = false;
            foreach (var childSpriteRenderer in childrenSpriteRenderer)
            {
                childSpriteRenderer.enabled = false;
            }
            foreach (var childCollider in childrenCollider)
            {
                childCollider.enabled = false;
            }
            room.CheckIfAllEnemiesDead();
            KillEnemyEventHandler?.Invoke(null, EventArgs.Empty);
        }

        /// Restore the health of this enemy based on the given health amount.
        /// ATTENTION: This method can be called only by a healer enemy.
        public bool Heal(int health)
        {
            // Healers cannot cure other healers
            if (weaponPrefab != null &&
                weaponPrefab.name.Contains(HealerPrefabName))
            {
                return false;
            }
            // Heal this enemy
            return healthController.ApplyHeal(health);
        }

        public float GetAttackSpeed()
        {
            return attackSpeed;
        }

        protected void UpdateAnimation(Vector2 movementDirection)
        {
            if (movementDirection.x == 0f && movementDirection.y == 0f)
            {
                animator.SetFloat(LastDirX, lastX);
                animator.SetFloat(LastDirY, lastY);
                animator.SetBool(IsMoving, false);
            }
            else
            {
                lastX = movementDirection.x;
                lastY = movementDirection.y;
                animator.SetBool(IsMoving, true);
            }
            animator.SetFloat(DirX, movementDirection.x);
            animator.SetFloat(DirY, movementDirection.y);
        }

        public void LoadEnemyData(EnemySO enemyData)
        {
            if (!_hasGotComponents)
            {
                GetAllComponents();
            }
            healthController.SetHealth(enemyData.health);
            damage = enemyData.damage;
            movementSpeed = enemyData.movementSpeed;
            restTime = enemyData.restTime;
            activeTime = enemyData.activeTime;
            attackSpeed = enemyData.attackSpeed;
            projectileSpeed = enemyData.projectileSpeed * 4;


            if (enemyData.weapon.name == "Shield")
                weaponPrefab = Instantiate(enemyData.weapon.weaponPrefab, shieldSpawn.transform);
            else if (enemyData.weapon.name != "None")
                weaponPrefab = Instantiate(enemyData.weapon.weaponPrefab, weaponSpawn.transform);
            hasProjectile = enemyData.weapon.hasProjectile;
            if (hasProjectile)
            {
                projectilePrefab = enemyData.weapon.projectile.projectilePrefab;
                projectileType = enemyData.weapon.projectile;
            }
            else
                projectilePrefab = null;
            movement = enemyData.movement;
            behavior = enemyData.behavior.enemyBehavior;
            if (enemyData.weapon.hasProjectile || enemyData.weapon.name == "Cure")
            {
                ApplyMageHat();
            }
            else if (enemyData.weapon.name == "None")
            {
                ApplySlimeSprite();
            }
            // ApplyEnemyColors();
            hasMoveDirBeenChosen = false;
            originalColor = spriteRenderer.color;
            healthController.SetOriginalColor(originalColor);
            if (hasProjectile && projectilePrefab.name == "EnemyBomb")
            {
                attackSpeed /= 2;
            }
            //If the movement needs to be fixed for the whole active time, set the flag here
            if (movement.enemyMovementIndex is Enums.MovementEnum.Random or Enums.MovementEnum.Random1D || movement.enemyMovementIndex == Enums.MovementEnum.Flee1D || movement.enemyMovementIndex == Enums.MovementEnum.Follow1D)
                hasFixedMoveDir = true;
            else
                hasFixedMoveDir = false;
            isWalking = false;
            isShooting = false;
            waitUntil = 0.5f;
            coolDownTime = 0.5f;
            dataHasBeenLoaded = true;
        }

        private void ApplyMageHat()
        {
            if (headSpriteRenderer == null) return;
            switch (movement.enemyMovementIndex)
            {
                case Enums.MovementEnum.Random:
                case Enums.MovementEnum.Random1D:
                    headSpriteRenderer.sprite = RandomMovementSprite;
                    break;
                case Enums.MovementEnum.Flee1D:
                case Enums.MovementEnum.Flee:
                    headSpriteRenderer.sprite = FleeMovementSprite;
                    break;
                case Enums.MovementEnum.Follow1D:
                case Enums.MovementEnum.Follow:
                    headSpriteRenderer.sprite = FollowMovementSprite;
                    break;
                case Enums.MovementEnum.None:
                    headSpriteRenderer.sprite = NoneMovementSprite;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }    
    
        private void ApplySlimeSprite()
        {
            switch (movement.enemyMovementIndex)
            {
                case Enums.MovementEnum.Random:
                case Enums.MovementEnum.Random1D:
                    spriteRenderer.sprite = RandomMovementSprite;
                    break;
                case Enums.MovementEnum.Flee1D:
                case Enums.MovementEnum.Flee:
                    spriteRenderer.sprite = FleeMovementSprite;
                    break;
                case Enums.MovementEnum.Follow1D:
                case Enums.MovementEnum.Follow:
                    spriteRenderer.sprite = FollowMovementSprite;
                    break;
                case Enums.MovementEnum.None:
                    spriteRenderer.sprite = NoneMovementSprite;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Movement Enum does not exist");
            }
        }
    
        private void ApplyEnemyColors()
        {

            originalColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(healthController.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 0, 1.0f) / 1.0f, 1.0f);
            //originalColor = new Color(, 0, 1 - Util.LogNormalization(healthCtrl.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 30, 225)/225f);
            armsColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 0, 1.0f) / 1.0f, 1.0f);
            legsColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 0, 1.0f) / 1.0f, 1.0f);
            //armsColor = new Color(Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f);
            //legsColor = new Color(Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f);
            //TODO change head color according to movement
            headColor = originalColor;
            spriteRenderer.color = originalColor;
            if (armSpriteRenderer != null)
            {
                armSpriteRenderer.color = armsColor;
            }
            if (legSpriteRenderer != null)
            {
                legSpriteRenderer.color = legsColor;
            }
            if (headSpriteRenderer != null)
            {
                headSpriteRenderer.color = headColor;
            }
        }
        
        public void SetRoom(RoomBhv _room)
        {
            room = _room;
        }

        protected void Shoot()
        {
            Vector2 target = new Vector2(playerObj.transform.position.x - transform.position.x, playerObj.transform.position.y - transform.position.y);
            target.Normalize();
            target *= projectileSpeed;


            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
            if (projectilePrefab.name == "EnemyBomb")
            {
                var bombController = bullet.GetComponent<BombController>();
                bombController.Damage = damage;
                bombController.SetEnemyThatShot(indexOnEnemyList);
                bombController.Shoot(target);
            }
            else
            {
                var projectileController = bullet.GetComponent<ProjectileController>();
                //bullet.GetComponent<ProjectileController>().damage = damage;
                projectileController.SetEnemyThatShot(indexOnEnemyList);
                projectileController.ProjectileSo = projectileType;
                projectileController.Shoot(target);
            }
        }
    }
}
