using UnityEngine;
using ScriptableObjects;

public class SpaceShooterEnemy : MonoBehaviour
{
    public bool isTest = false;
    public EnemySO enemySO;

    [Header("Runtime Stats")]
    public float moveSpeed;
    public float lifePoints;
    public float attackSpeed;
    public float waitTime;
    public float projectileSpeed;

    private float spawnTime;

    [SerializeField] private EnemySpriteResolver spriteResolver;
    private SpaceShooterMovement movement;
    private SpaceShooterWeapon weapon;

    void Start()
    {
        if (isTest)
        {
            Init(
                enemySO,
                EnemyLoader.CreateMovement(this.gameObject, enemySO.movement),
                EnemyLoader.CreateWeapon(this.gameObject, enemySO.weapon));
        }
        spawnTime = Time.time;
    }

    public void Init(
        EnemySO so,
        SpaceShooterMovement movementType,
        SpaceShooterWeapon weaponType)
    {
        enemySO = so;

        moveSpeed = so.status1;
        lifePoints = so.status2;
        attackSpeed = so.status3;
        waitTime = so.status4;
        projectileSpeed = so.weaponStatus1;

        movement = movementType;
        weapon = weaponType;

        spriteResolver.ApplySprite(movement, weapon);

        movement.Init(this);
        weapon.Init(this);
    }

    void Update()
    {
        if (Time.time - spawnTime < waitTime)
            return;

        movement.Tick();
        if (gameObject.GetComponent<MoveVerticalOnly>() == null)
            weapon.Tick();
    }

    public void Die()
    {
        StatusManager.Instance.SumEnemyDefeated();
        FindObjectOfType<RoomController>()?.OnKilledEnemy();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponentInParent<PlayerShip>()?.TakeDamage();
            //col.GetComponent<PlayerShip>()?.TakeDamage();
            Die();
        }
        else if (col.CompareTag("PlayerProjectile"))
        {
            //lifePoints -= col.GetComponent<PlayerBullet>()?.damage ?? 1;
            lifePoints -= FindObjectOfType<PlayerShip>()?.damagePerBullet ?? 1;
            Destroy(col.gameObject);
            if (lifePoints <= 0)
            {
                ScoreManager.Instance.AddScore(10);
                Die();
            }
        }
    }
}
