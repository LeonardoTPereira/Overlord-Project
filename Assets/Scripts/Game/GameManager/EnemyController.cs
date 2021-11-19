using Game.EnemyGenerator;
using System;
using ScriptableObjects;
using UnityEngine;
using Util;

public class EnemyController : MonoBehaviour
{
    /// This constant holds the weapon prefab name of healers
    public static readonly string HEALER_PREFAB_NAME = "EnemyHealArea";
    private static readonly int HIT_ENEMY = 0;
    private static readonly int ENEMY_DEATH = 1;

    protected bool isActive;
    protected bool canDestroy;
    [SerializeField]
    protected float restTime, activeTime, movementSpeed, attackSpeed, projectileSpeed;
    protected int damage;
    [SerializeField]
    protected GameObject playerObj, bloodParticle, weaponPrefab, projectilePrefab, projectileSpawn, weaponSpawn, shieldSpawn;
    [SerializeField]
    protected MovementTypeSO movement;
    protected BehaviorType behavior;
    protected ProjectileTypeSO projectileType;

    protected Animator anim;
    private AudioSource[] audioSrcs;
    [SerializeField]
    protected float walkUntil, waitUntil, coolDownTime;
    protected bool isWalking, hasProjectile, isShooting;
    [SerializeField]
    protected bool hasMoveDirBeenChosen, hasFixedMoveDir, dataHasBeenLoaded;
    protected Color originalColor, armsColor, headColor, legsColor;
    protected float lastX, lastY;
    [SerializeField]
    protected Vector3 targetMoveDir;
    protected RoomBhv room;
    [SerializeField]
    protected int indexOnEnemyList;
    protected HealthController healthCtrl;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    public static event EventHandler playerHitEventHandler;
    public static event EventHandler KillEnemyEventHandler;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
        isActive = true;
        canDestroy = false;
        dataHasBeenLoaded = false;
        playerObj = Player.Instance.gameObject;
        anim = GetComponent<Animator>();
        audioSrcs = GetComponents<AudioSource>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        healthCtrl = gameObject.GetComponent<HealthController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        ProjectileController.enemyHitEventHandler += HurtEnemy;
        PlayerController.PlayerDeathEventHandler += PlayerHasDied;
    }

    void OnDisable()
    {
        ProjectileController.enemyHitEventHandler -= HurtEnemy;
        PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
    }

    protected virtual void OnPlayerHit()
    {
        playerHitEventHandler?.Invoke(null, EventArgs.Empty);
    }

    private void HurtEnemy(object sender, EventArgs eventArgs)
    {
        if (healthCtrl.GetHealth() > 0 && !audioSrcs[HIT_ENEMY].isPlaying)
        {
            audioSrcs[HIT_ENEMY].PlayOneShot(audioSrcs[HIT_ENEMY].clip, 1.0f);
        }
    }

    private void PlayerHasDied(object sender, EventArgs eventArgs)
    {
        isActive = false;
    }

    void Update()
    {
        if (!audioSrcs[ENEMY_DEATH].isPlaying && canDestroy)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///
    /// </summary>
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dataHasBeenLoaded && isActive && !canDestroy)
        {
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
    }

    void Walk()
    {
        if (!hasMoveDirBeenChosen)
        {
            int xOffset, yOffset;
            //Vector2 target = new Vector2(playerObj.transform.position.x - transform.position.x, playerObj.transform.position.y - transform.position.y);
            if (movement.movementType == null)
                Debug.LogError("NO MOVEMENT FUNCTION!");
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
        //transform.position += new Vector3((target.x + xOffset) * movementSpeed * Time.deltaTime, (target.y + yOffset) * movementSpeed * Time.deltaTime, 0f);
        walkUntil -= Time.deltaTime;
    }

    void Wait()
    {
        //TODO Scream
        rb.velocity = Vector3.zero;
        waitUntil -= Time.deltaTime;
    }

    void WaitShotCoolDown()
    {
        coolDownTime -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerHit();
            collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage, indexOnEnemyList);
        }
    }

    public void CheckDeath()
    {
        if (healthCtrl.GetHealth() <= 0f)
        {
            //TODO Audio and Particles
            //Instantiate(bloodParticle, transform.position, Quaternion.identity);
            audioSrcs[ENEMY_DEATH].PlayOneShot(audioSrcs[ENEMY_DEATH].clip, 1.0f);
            canDestroy = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            room.CheckIfAllEnemiesDead();
            KillEnemyEventHandler?.Invoke(null, EventArgs.Empty);
        }
    }

    /// Restore the health of this enemy based on the given health amount.
    /// ATTENTION: This method can be called only by a healer enemy.
    public bool Heal(int health)
    {
        // Healers cannot cure other healers
        if (weaponPrefab != null &&
            weaponPrefab.name.Contains(HEALER_PREFAB_NAME))
        {
            return false;
        }
        // Heal this enemy
        return healthCtrl.ApplyHeal(health);
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    protected void UpdateAnimation(Vector2 movement)
    {
        if (movement.x == 0f && movement.y == 0f)
        {
            anim.SetFloat("LastDirX", lastX);
            anim.SetFloat("LastDirY", lastY);
            anim.SetBool("IsMoving", false);
        }
        else
        {
            lastX = movement.x;
            lastY = movement.y;
            anim.SetBool("IsMoving", true);
        }
        anim.SetFloat("DirX", movement.x);
        anim.SetFloat("DirY", movement.y);
    }
    /// <summary>
    /// Loads the enemy data.
    /// </summary>
    /// <param name="enemyData">The enemy data.</param>
    /// <param name="index">The index.</param>
    public void LoadEnemyData(EnemySO enemyData)
    {
        healthCtrl.SetHealth(enemyData.health);
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
        // ApplyEnemyColors();
        hasMoveDirBeenChosen = false;
        originalColor = sr.color;
        healthCtrl.SetOriginalColor(originalColor);
        if (hasProjectile && projectilePrefab.name == "EnemyBomb")
        {
            attackSpeed /= 2;
        }
        //If the movement needs to be fixed for the whole active time, set the flag here
        if (movement.enemyMovementIndex == Enums.MovementEnum.Random || movement.enemyMovementIndex == Enums.MovementEnum.Random1D || movement.enemyMovementIndex == Enums.MovementEnum.Flee1D || movement.enemyMovementIndex == Enums.MovementEnum.Follow1D)
            hasFixedMoveDir = true;
        else
            hasFixedMoveDir = false;
        isWalking = false;
        isShooting = false;
        waitUntil = 0.5f;
        coolDownTime = 0.5f;
        dataHasBeenLoaded = true;
    }

    private void ApplyEnemyColors()
    {

        originalColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(healthCtrl.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 0, 1.0f) / 1.0f, 1.0f);
        //originalColor = new Color(, 0, 1 - Util.LogNormalization(healthCtrl.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 30, 225)/225f);
        armsColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 0, 1.0f) / 1.0f, 1.0f);
        legsColor = Color.HSVToRGB(0.0f, Constants.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 0, 1.0f) / 1.0f, 1.0f);
        //armsColor = new Color(Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f);
        //legsColor = new Color(Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f);
        //TODO change head color according to movement
        headColor = originalColor;
        sr.color = originalColor;
        SpriteRenderer arms = gameObject.transform.Find("EnemyArms").GetComponent<SpriteRenderer>();
        if (arms != null)
        {
            arms.color = armsColor;
        }
        SpriteRenderer legs = gameObject.transform.Find("EnemyLegs").GetComponent<SpriteRenderer>();
        if (legs != null)
        {
            legs.color = legsColor;
        }
        SpriteRenderer head = gameObject.transform.Find("EnemyHead").GetComponent<SpriteRenderer>();
        if (head != null)
        {
            head.color = headColor;
        }
    }

    /// <summary>
    /// Sets the room.
    /// </summary>
    /// <param name="_room">The room.</param>
    public void SetRoom(RoomBhv _room)
    {
        room = _room;
    }
    /// <summary>
    /// Shoots this instance.
    /// </summary>
    protected void Shoot()
    {
        Vector2 target = new Vector2(playerObj.transform.position.x - transform.position.x, playerObj.transform.position.y - transform.position.y);
        target.Normalize();
        target *= projectileSpeed;


        GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
        //bullet.GetComponent<Rigidbody2D>().AddForce(target, ForceMode2D.Impulse);
        if (projectilePrefab.name == "EnemyBomb")
        {
            //Debug.Log("It's a bomb");
            bullet.GetComponent<BombController>().damage = damage;
            bullet.GetComponent<BombController>().SetEnemyThatShot(indexOnEnemyList);

        }
        else
        {
            //bullet.GetComponent<ProjectileController>().damage = damage;
            bullet.GetComponent<ProjectileController>().SetEnemyThatShot(indexOnEnemyList);
            bullet.GetComponent<ProjectileController>().ProjectileSO = projectileType;
        }
        bullet.SendMessage("Shoot", target);
    }

    //TODO method to shoot a bomb
}
