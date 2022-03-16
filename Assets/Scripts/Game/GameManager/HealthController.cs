using Game.Events;
using UnityEngine;

namespace Game.GameManager
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int health;
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

        private void Update()
        {
            if (!isInvincible) return;
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

        public void ApplyDamage(int damage, Vector3 impactDirection, int enemyIndex = -1)
        {
            if (isInvincible) return;
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
        
        public bool ApplyHeal(int healing)
        {
            if (GetMaxHealth() <= GetHealth()) return false;
            var newHealth = health + healing;
            health = maxHealth >= newHealth ? newHealth : maxHealth;
            return true;
        }

        public void SetHealth(int newHealth)
        {
            if (maxHealth == -1)
            {
                maxHealth = newHealth;
            }
            health = newHealth;
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

        public void SetOriginalColor(Color color)
        {
            originalColor = color;
        }
    }
}
