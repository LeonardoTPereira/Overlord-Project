using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    int health;
    int maxHealth;
    bool isInvincible;
    float invincibilityTime, invincibilityCount;
    Color originalColor;

    public static event PlayerIsDamagedEvent PlayerIsDamagedEventHandler;

    private void Awake()
    {
        maxHealth = -1;
        isInvincible = false;
        invincibilityCount = 0f;
        invincibilityTime = 0.2f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            if (invincibilityTime < invincibilityCount)
            {
                isInvincible = false;
                gameObject.GetComponent<SpriteRenderer>().color = originalColor;
            }
            else
            {
                invincibilityCount += Time.deltaTime;
            }
        }
    }

    public void ApplyDamage(int damage, int enemyIndex = -1)
    {
        if (!isInvincible)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            health -= damage;
            isInvincible = true;
            invincibilityCount = 0f;
            if (gameObject.CompareTag("Player"))
            {
                PlayerIsDamagedEventHandler(this, new PlayerIsDamagedEventArgs(enemyIndex, damage, health));
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                gameObject.GetComponent<EnemyController>().CheckDeath();
            }
        }
    }

    public void SetHealth(int _health)
    {
        health = _health;
        if (maxHealth == -1)
        {
            maxHealth = _health;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetOriginalColor(Color _color)
    {
        originalColor = _color;
    }
}
