using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    [SerializeField]
    private AudioClip popSnd;
    private AudioSource audioSrc;
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D collider;

    private bool canDestroy, hasBeenThrown, hasTimerBeenSet, isExploding;
    public int damage, enemyThatShot;
    [SerializeField]
    protected float bombLifetime;
    protected float bombCountdown;

    public delegate void HitPlayerEvent();
    public static event HitPlayerEvent hitPlayerEvent;

    protected bool isColliding;

    // Use this for initialization
    void Awake()
    {
        bombLifetime = 2.0f;
        canDestroy = false;
        hasBeenThrown = false;
        hasTimerBeenSet = false;
        isExploding = false;
        audioSrc = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBeenThrown && !hasTimerBeenSet)
        {
            bombCountdown = bombLifetime;
            hasTimerBeenSet = true;
        }
        if(hasTimerBeenSet && !isExploding)
        {
            if (bombCountdown >= 0.01f)
                bombCountdown -= Time.deltaTime;
            else
                ExplodeBomb();
            if(!isColliding)
                if (bombCountdown < (bombLifetime-0.2f))
                {
                    isColliding = true;
                    collider.enabled = isColliding;
                }
        }
        if (!audioSrc.isPlaying && canDestroy)
        {
            //Debug.Log("Stopped playing");
            Destroy(gameObject);
        }
    }

    public void DestroyBomb()
    {
        //Debug.Log("Destroying Bomb");
        canDestroy = true;
    }

    public void SetEnemyThatShot(int _index)
    {
        enemyThatShot = _index;
    }

    protected virtual void OnPlayerHit()
    {
        hitPlayerEvent();
    }

    public void Shoot(Vector2 facingDirection)
    {
        isColliding = false;
        collider.enabled = isColliding;
        
        rb.AddForce(facingDirection, ForceMode2D.Impulse);
        hasBeenThrown = true;
    }

    private bool CheckIfStopped()
    {
        if(rb.velocity.magnitude < 5f)
        {
            return true;
        }
        return false;
    }

    private void ExplodeBomb()
    {
        animator.SetTrigger("Explode");
        audioSrc.PlayOneShot(popSnd, 0.3f);
        isExploding = true;
        Vector3 currScale = transform.localScale;
        transform.localScale = new Vector3(currScale.x*4, currScale.y*4, currScale.z*1);
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(new Vector2 (rb.position.x, rb.position.y), 1.8f);
        foreach (Collider2D col in objectsInRange)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                OnPlayerHit();
                col.gameObject.GetComponent<HealthController>().ApplyDamage(damage, enemyThatShot);
            }
        }
    }
}
