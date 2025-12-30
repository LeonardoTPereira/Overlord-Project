using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float slowMoveMultiplier = 0.4f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f;

    [Header("Lives")]
    public int maxLives = 3;
    public float invincibilityTime = 2f;

    [Header("Bomb")]
    public int maxBombs = 3;
    public float bombInvincibilityTime = 3f;

    [Header("Invincibility Visuals")]
    [Range(0f, 1f)]
    public float blinkMinAlpha = 0.3f;
    public float blinkInterval = 0.1f;

    [Header("Screen Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [SerializeField]

    private int _currentLives;
    private int _currentBombs;

    private bool _isInvincible;
    private float _fireTimer;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _col;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _currentLives = maxLives;
        _currentBombs = maxBombs;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleBomb();
    }

    #region Movement

    private bool _isMovingUp;
    private bool _isMovingDown;
    private bool _isMovingLeft;
    private bool _isMovingRight;
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isMovingUp = true;
        }
        else if (context.canceled)
        {
            _isMovingUp = false;
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isMovingDown = true;
        }
        else if (context.canceled)
        {
            _isMovingDown = false;
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isMovingLeft = true;
        }
        else if (context.canceled)
        {
            _isMovingLeft = false;
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isMovingRight = true;
        }
        else if (context.canceled)
        {
            _isMovingRight = false;
        }
    }

    private bool _isSlowing = false;
    public void OnSlow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSlowing = true;
        }
        else if (context.canceled)
        {
            _isSlowing = false;
        }
    }    
    void HandleMovement()
    {
        Vector2 direction = Vector2.zero;

        if(_isMovingLeft) direction.x -= 1f;
        if(_isMovingRight) direction.x += 1f;
        if(_isMovingUp) direction.y += 1f;
        if(_isMovingDown) direction.y -= 1f;

        direction = Vector2.ClampMagnitude(direction, 1f);

        float speed = moveSpeed;
        if (_isSlowing)
            speed *= slowMoveMultiplier;

        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        transform.position = newPos;
    }    

    #endregion

    #region Shooting

    private bool _isShooting = false;
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;
        }
    }

    void HandleShooting()
    {
        _fireTimer -= Time.deltaTime;

        if (_isShooting && _fireTimer <= 0f)
        {
            Shoot();
            _fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
    #endregion

    #region Bomb
    void HandleBomb()
    {
        if (Input.GetKeyDown(KeyCode.X) && _currentBombs > 0)
        {
            UseBomb();
        }
    }

    void UseBomb()
    {
        _currentBombs--;

        DestroyAllWithTag("Enemy");
        DestroyAllWithTag("EnemyBullet");
        DestroyAllWithTag("Obstacle");

        StartCoroutine(Invincibility(bombInvincibilityTime));
    }

    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
    #endregion

    #region Damage & Invincibility
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isInvincible)
            return;

        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        _currentLives--;

        if (_currentLives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility(invincibilityTime));
        }
    }

    IEnumerator Invincibility(float duration)
    {
        if (_isInvincible)
            yield break;

        _isInvincible = true;
        _col.enabled = false;

        float timer = 0f;
        bool faded = false;

        Color baseColor = _spriteRenderer.color;

        while (timer < duration)
        {
            faded = !faded;
            float alpha = faded ? blinkMinAlpha : 1f;

            _spriteRenderer.color = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                alpha
            );

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        _spriteRenderer.color = new Color(
            baseColor.r,
            baseColor.g,
            baseColor.b,
            1f
        );

        _col.enabled = true;
        _isInvincible = false;
    }

    /*
    IEnumerator InvincibilityCoroutine(float duration)
    {
        _isInvincible = true;
        _col.enabled = false;

        float timer = 0f;
        while (timer < duration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        _spriteRenderer.enabled = true;
        _col.enabled = true;
        _isInvincible = false;
    }
    */
    void Die()
    {
        // TODO: explosion, game over, respawn logic
        Destroy(gameObject);
    }
    #endregion
}
