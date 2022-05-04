using Game.Events;
using UnityEngine;

namespace Game.GameManager
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int health;
        private int maxHealth;
        private bool isInvincible;
        private float invincibilityTime;
        private float invincibilityCount;
        private Color originalColor;
        private SpriteRenderer spriteRenderer;
        private EnemyController enemyController;

        public static event PlayerIsDamagedEvent PlayerIsDamagedEventHandler;

        private void Start()
        {
            enemyController = gameObject.GetComponent<EnemyController>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            maxHealth = -1;
            isInvincible = false;
            invincibilityCount = 0f;
            invincibilityTime = 0.2f;
        }

        //TODO change invincibility timer to coroutine
        private void Update()
        {
            if (!isInvincible) return;
            if (invincibilityTime < invincibilityCount)
            {
                isInvincible = false;
                spriteRenderer.color = originalColor;
            }
            else
            {
                invincibilityCount += Time.deltaTime;
            }
        }

        public void ApplyDamage(int damage, Vector3 impactDirection, int enemyIndex = -1)
        {
            if (isInvincible) return;
            spriteRenderer.color = Color.red;
            health -= damage;
            isInvincible = true;
            invincibilityCount = 0f;
            if (gameObject.CompareTag("Player"))
            {
                PlayerIsDamagedEventHandler?.Invoke(this, new PlayerIsDamagedEventArgs(enemyIndex, damage, health, impactDirection));
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                enemyController.CheckDeath();
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
