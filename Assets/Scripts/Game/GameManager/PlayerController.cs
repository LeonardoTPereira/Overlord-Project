﻿using EnemyGenerator;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    protected float speed, shootSpeed, coolDownTime, atkSpeed;
    [SerializeField]
    protected int shootDmg, maxHealth;
    [SerializeField]
    protected GameObject bulletSpawn, bulletPrefab;


    Animator anim;
    float lastX, lastY;
    private AudioSource audioSrc;

    [SerializeField]
    private float timeAfterShoot, rotatedAngle;
    private Vector2 shootForce = new Vector2(0f, 0f);
    private Color originalColor;
    protected Rigidbody2D rb;

    private int actualProjectile;

    HealthController healthCtrl;

    [SerializeField]
    private ProjectileTypeSO projectileType;

    public static event EventHandler PlayerDeathEventHandler;

    public void Awake()
    {
        actualProjectile = 0;
        projectileType = GameManager.instance.projectileSet.Items[actualProjectile];
        anim = GetComponent<Animator>();
        timeAfterShoot = 0.0f;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        healthCtrl = gameObject.GetComponent<HealthController>();
        healthCtrl.SetOriginalColor(originalColor);
        rb = gameObject.GetComponent<Rigidbody2D>();
        SetProjectileSO(null, new LoadWeaponButtonEventArgs(GameManager.instance.projectileType));
    }

    // Use this for initialization
    void Start()
    {
        healthCtrl.SetHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        WeaponLoaderBHV.LoadWeaponButtonEventHandler += SetProjectileSO;
        HealthController.PlayerIsDamagedEventHandler += CheckDeath;
    }
    private void OnDisable()
    {
        WeaponLoaderBHV.LoadWeaponButtonEventHandler -= SetProjectileSO;
        HealthController.PlayerIsDamagedEventHandler -= CheckDeath;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();

        float shootHorizontal = Input.GetAxisRaw("HorizontalShoot");
        float shootVertical = Input.GetAxisRaw("VerticalShoot");
        Vector2 shootDir = new Vector2(shootHorizontal, shootVertical);
        shootDir.Normalize();

        if (Input.GetKeyDown(KeyCode.Tab))
            NextProjectileSO();

        Move(movement, shootDir);

        if (coolDownTime > 0.0f)
        {
            coolDownTime -= Time.fixedDeltaTime;
        }
        else
        {
            Shoot(shootDir, movement);
        }
    }

    protected void Move(Vector2 movement, Vector2 shoot)
    {
        if (movement.magnitude > 0.01f)
            transform.position += (Vector3)movement * speed * Time.fixedDeltaTime;
        else
            rb.velocity = Vector3.zero;
        UpdateMoveAnimation(movement, shoot);
        /*if(movement != Vector2.zero){
			movement.x *= 100000; //favorece olhar na horizontal (elimina olhar diagonal)
			transform.up = movement;	
		}*/
    }

    protected void Shoot(Vector2 shootDir, Vector2 movementDir)
    {
        bool willShoot = false;
        if (shootDir.x > 0.01f)
        {
            rotatedAngle = 0;
            shootForce = new Vector2(shootSpeed, 0f);
            willShoot = true;
        }
        else if (shootDir.x < -0.01f)
        {
            rotatedAngle = 180;
            shootForce = new Vector2(-shootSpeed, 0f);
            willShoot = true;
        }
        else if (shootDir.y > 0.01f)
        {
            rotatedAngle = 90;
            shootForce = new Vector2(0f, shootSpeed);
            willShoot = true;
        }
        else if (shootDir.y < -0.01f)
        {
            rotatedAngle = 270;
            shootForce = new Vector2(0f, -shootSpeed);
            willShoot = true;
        }
        bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, rotatedAngle);
        if (willShoot)
        {

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.tag = "Bullet"; //adicionado por Luana e Paolo
            bullet.GetComponent<ProjectileController>().ProjectileSO = projectileType;
            bullet.SendMessage("Shoot", shootForce + movementDir);
            bulletSpawn.transform.RotateAround(transform.position, Vector3.forward, rotatedAngle + 90);
            bullet.GetComponent<Rigidbody2D>().AddForce(shootForce + movementDir, ForceMode2D.Impulse);
            //bullet.GetComponent<ProjectileController>().damage = this.shootDmg;
            bulletSpawn.transform.RotateAround(transform.position, Vector3.forward, -rotatedAngle);
            coolDownTime = 1.0f / atkSpeed;
        }
    }

    protected void UpdateMoveAnimation(Vector2 movement, Vector2 shoot)
    {
        Vector2 directionToFace;
        //If it is shooting, the animation direction takes priority over movement
        if (System.Math.Abs(shoot.x) > 0.01f || System.Math.Abs(shoot.y) > 0.01f)
        {
            anim.SetFloat("LastDirX", lastX);
            anim.SetFloat("LastDirY", lastY);
            //Debug.Log("Shooting Direction");
            directionToFace = shoot;
        }
        //Else, will face the movement direction
        else
        {
            directionToFace = movement;
        }
        //If not shooting nor moving, maintain the idle direction
        if (directionToFace.x == 0f && directionToFace.y == 0f)
        {
            anim.SetFloat("LastDirX", lastX);
            anim.SetFloat("LastDirY", lastY);
        }
        //Else, update the idle direction
        else
        {
            lastX = directionToFace.x;
            lastY = directionToFace.y;
        }
        //If not moving, sets the animation boolean to false
        if (movement.x == 0f && movement.y == 0f)
        {
            anim.SetBool("IsMoving", false);
        }
        //Else, to true and updates the movement direction
        else
        {

            anim.SetFloat("DirX", directionToFace.x);
            anim.SetFloat("DirY", directionToFace.y);
            anim.SetBool("IsMoving", true);
        }
    }

    public void PlayGetkey()
    {
        audioSrc.PlayOneShot(audioSrc.clip, 1.0f);
    }

    private void CheckDeath(object sender, PlayerIsDamagedEventArgs eventArgs)
    {
        if (eventArgs.PlayerHealth <= 0)
        {
            //TODO KILL
            Time.timeScale = 0f;
            PlayerDeathEventHandler?.Invoke(this, EventArgs.Empty);
            //Debug.Log("RIP");
        }
    }

    public void SetProjectileSO(object sender, LoadWeaponButtonEventArgs eventArgs)
    {
        projectileType = eventArgs.ProjectileSO;
        bulletPrefab = projectileType.projectilePrefab;
        bulletPrefab.GetComponent<ProjectileController>().ProjectileSO = projectileType;
        atkSpeed = projectileType.atkSpeed;
        bulletPrefab.GetComponent<SpriteRenderer>().color = eventArgs.ProjectileSO.color;
    }

    public void ResetHealth()
    {
        healthCtrl.SetHealth(maxHealth);
    }

    public int GetHealth()
    {
        return healthCtrl.GetHealth();
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void NextProjectileSO()
    {
        actualProjectile = (actualProjectile + 1) % GameManager.instance.projectileSet.Items.Count;
        projectileType = GameManager.instance.projectileSet.Items[actualProjectile];
        SetProjectileSO(this, new LoadWeaponButtonEventArgs(projectileType));
    }

}