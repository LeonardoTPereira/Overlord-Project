using System;
using Game.Events;
using UnityEngine;
using UnityEngine.UIElements;

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

    public void ApplyDamage(int damage, Vector3 impactDirection, int enemyIndex = -1)
    {
        if (!isInvincible)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            health -= damage;
            isInvincible = true;
            invincibilityCount = 0f;
            if (gameObject.CompareTag("Player"))
            {
                PlayerIsDamagedEventHandler?.Invoke(this, new PlayerIsDamagedEventArgs(enemyIndex, damage, health, impactDirection));
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                gameObject.GetComponent<EnemyController>().CheckDeath();
            }
        }
    }

    /// This method restores the health with the given amount of health when
    /// the health is lesser than the max health. Return true if the enemy was
    /// healed, and false otherwise.
    public bool ApplyHeal(int _health)
    {
        // If the enemy is injured, then heal it; if not, ignore it
        if (GetMaxHealth() > GetHealth())
        {
            // Calculate the new health
            int newHealth = health + _health;
            // The new health cannot be higher than the max health
            health = maxHealth >= newHealth ? newHealth : maxHealth;
            return true;
        }
        return false;
    }

    public void SetHealth(int _health)
    {
        // If not initialized, then define the max health
        if (maxHealth == -1)
        {
            maxHealth = _health;
        }
        health = _health;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void SetOriginalColor(Color _color)
    {
        originalColor = _color;
    }
}
