
using EnemyGenerator;
using UnityEngine;
using static Util;

public class ProjectileController : MonoBehaviour
{

    [SerializeField]
    private AudioClip popSnd;
    private AudioSource audioSrc;
    private Rigidbody2D rb;

    private bool canDestroy, isSin;
    public int enemyThatShot;

    public delegate void HitEnemyEvent();
    public static event HitEnemyEvent hitEnemyEvent;
    public delegate void HitPlayerEvent();
    public static event HitPlayerEvent hitPlayerEvent;

    private Vector2 pos, moveDir;
    private float MoveSpeed { get; set; }

    [SerializeField]
    private float frequency, magnitude;
    [SerializeField]
    private int damage;
    [SerializeField]
    public ProjectileTypeSO ProjectileSO { get; set; }

    // Use this for initialization
    void Awake()
    {
        canDestroy = false;
        audioSrc = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        isSin = false;
    }

    private void Start()
    {
        if (ProjectileSO == null)
            Debug.LogError("NO PROJECTILE SO!!!");
        MoveSpeed = ProjectileSO.moveSpeed;
        damage = ProjectileSO.damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSrc.isPlaying && canDestroy)
        {
            Destroy(gameObject);
        }
        if(isSin)
        {
            pos += moveDir * Time.deltaTime * MoveSpeed;
            transform.position = pos + Vector2.Perpendicular(moveDir).normalized * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }

    public void DestroyBullet()
    {
        //Debug.Log("Destroying Bullet");
        audioSrc.PlayOneShot(popSnd, 0.15f);
        canDestroy = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    protected virtual void OnEnemyHit()
    {
        hitEnemyEvent();
    }

    protected virtual void OnPlayerHit()
    {
        hitPlayerEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("EnemyBullet"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerHit();
                collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage, enemyThatShot);
                DestroyBullet();
            }
        }
        else if(CompareTag("Bullet"))
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                OnEnemyHit();
                collision.gameObject.GetComponent<HealthController>().ApplyDamage(damage);
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

    public void SetEnemyThatShot(int _index)
    {
        enemyThatShot = _index;
    }

    public void Shoot(Vector2 facingDirection)
    {
        PlayerProjectileEnum projEnum = ProjectileSO.projectileBehaviorIndex;
        switch (projEnum)
        {
            case PlayerProjectileEnum.STRAIGHT:
                StraightShot(facingDirection);
                break;
            case PlayerProjectileEnum.SIN:
                SinShot(facingDirection);
                break;
            case PlayerProjectileEnum.TRIPLE:
                TripleShot(facingDirection);
                break;
        }
        //rb.AddForce(facingDirection, ForceMode2D.Impulse);
    }

    public void StraightShot(Vector2 facingDirection)
    {
        rb.AddForce(facingDirection, ForceMode2D.Impulse);
    }
    public void SinShot(Vector2 facingDirection)
    {
        pos = transform.position;
        moveDir = facingDirection.normalized;
        isSin = true;
    }

    public void TripleShot(Vector2 facingDirection)
    {
        Vector2 left, right;
        right = Quaternion.Euler(0, 0, 30) * facingDirection;
        left = Quaternion.Euler(0, 0, -30) * facingDirection;
        ProjectileController rightBullet, leftBullet;
        rightBullet = Instantiate(this, transform.position, transform.rotation);
        leftBullet = Instantiate(this, transform.position, transform.rotation);
        rightBullet.ProjectileSO = ProjectileSO;
        leftBullet.ProjectileSO = ProjectileSO;
        rightBullet.GetComponent<Rigidbody2D>().AddForce(right, ForceMode2D.Impulse);
        leftBullet.GetComponent<Rigidbody2D>().AddForce(left, ForceMode2D.Impulse);
        rb.AddForce(facingDirection, ForceMode2D.Impulse);
    }
}
