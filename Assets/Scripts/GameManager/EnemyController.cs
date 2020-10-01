using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGenerator;
using System;

public class EnemyController : MonoBehaviour
{
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
    [SerializeField]
    protected float walkUntil, waitUntil, coolDownTime;
    protected bool isWalking, hasProjectile, isShooting;
    [SerializeField]
    protected bool hasMoveDirBeenChosen, hasFixedMoveDir, dataHasBeenLoaded;
    protected Color originalColor, armsColor, headColor, legsColor;
    protected float lastX, lastY;
    [SerializeField]
    protected Vector3 targetMoveDir;
    protected RoomBHV room;
    [SerializeField]
    protected int indexOnEnemyList;
    protected HealthController healthCtrl;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    public delegate void HitPlayerEvent();
    public static event HitPlayerEvent hitPlayerEvent;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {

        dataHasBeenLoaded = false;
        playerObj = Player.instance.gameObject;
        anim = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        healthCtrl = gameObject.GetComponent<HealthController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void OnPlayerHit()
    {
        hitPlayerEvent();
    }
    /// <summary>
    /// 
    /// </summary>
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dataHasBeenLoaded)
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
            if(hasFixedMoveDir)
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
        if (collision.gameObject.tag == "Player")
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
            room.CheckIfAllEnemiesDead();
            Destroy(gameObject);
        }
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
    public void LoadEnemyData(EnemySO enemyData, int index)
    {
        healthCtrl.SetHealth(enemyData.health);
        damage = enemyData.damage;
        movementSpeed = enemyData.movementSpeed;
        restTime = enemyData.restTime;
        activeTime = enemyData.activeTime;
        attackSpeed = enemyData.attackSpeed;
        projectileSpeed = enemyData.projectileSpeed*4;


        if (enemyData.weapon.name == "Shield")
            weaponPrefab = Instantiate(enemyData.weapon.weaponPrefab, shieldSpawn.transform);
        else if(enemyData.weapon.name != "None")
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
        ApplyEnemyColors();
        indexOnEnemyList = index;
        hasMoveDirBeenChosen = false;
        originalColor = sr.color;
        healthCtrl.SetOriginalColor(originalColor);
        if (hasProjectile)
            if (projectilePrefab.name == "EnemyBomb")
                attackSpeed /= 2;
        //If the movement needs to be fixed for the whole active time, set the flag here
        if (movement.enemyMovementIndex == MovementEnum.Random || movement.enemyMovementIndex == MovementEnum.Random1D || movement.enemyMovementIndex == MovementEnum.Flee1D || movement.enemyMovementIndex == MovementEnum.Follow1D)
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
        
        originalColor = Color.HSVToRGB(0.0f, Util.LogNormalization(healthCtrl.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 0, 1.0f) / 1.0f, 1.0f);
        //originalColor = new Color(, 0, 1 - Util.LogNormalization(healthCtrl.GetHealth(), EnemyUtil.minHealth, EnemyUtil.maxHealth, 30, 225)/225f);
        armsColor = Color.HSVToRGB(0.0f, Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 0, 1.0f) / 1.0f, 1.0f);
        legsColor = Color.HSVToRGB(0.0f, Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 0, 1.0f) / 1.0f, 1.0f);
        //armsColor = new Color(Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(damage, EnemyUtil.minDamage, EnemyUtil.maxDamage, 30, 225)/ 225f);
        //legsColor = new Color(Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f, 0, 1 - Util.LogNormalization(movementSpeed, EnemyUtil.minMoveSpeed, EnemyUtil.maxMoveSpeed, 30, 225)/ 225f);
        //TODO change head color according to movement
        headColor = originalColor;
        sr.color = originalColor;
        gameObject.transform.Find("EnemyArms").GetComponent<SpriteRenderer>().color = armsColor;
        gameObject.transform.Find("EnemyLegs").GetComponent<SpriteRenderer>().color = legsColor;
        gameObject.transform.Find("EnemyHead").GetComponent<SpriteRenderer>().color = headColor;
    }

    /// <summary>
    /// Sets the room.
    /// </summary>
    /// <param name="_room">The room.</param>
    public void SetRoom(RoomBHV _room)
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
