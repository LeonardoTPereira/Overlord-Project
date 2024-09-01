using Game.Events;
using UnityEngine;
using Game.Quests;

namespace Game.GameManager
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int health;
        private int _maxHealth;
        private bool _isInvincible;
        private float _invincibilityTime;
        private float _invincibilityCount;
        private Color _originalColor;
        private SpriteRenderer _spriteRenderer;
        private EnemyController _enemyController;

        public static event PlayerIsDamagedEvent PlayerIsDamagedEventHandler;

        private void Start()
        {
            _enemyController = gameObject.GetComponent<EnemyController>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            _maxHealth = -1;
            _isInvincible = false;
            _invincibilityCount = 0f;
            _invincibilityTime = 0.2f;
        }

        //TODO change invincibility timer to coroutine
        private void Update()
        {
            if (!_isInvincible) return;
            if (_invincibilityTime < _invincibilityCount)
            {
                _isInvincible = false;
                _spriteRenderer.color = _originalColor;
            }
            else
            {
                _invincibilityCount += Time.deltaTime;
            }
        }

        public void ApplyDamage(int damage, Vector3 impactDirection, int enemyIndex = -1)
        {
            if (_isInvincible) return;
            _spriteRenderer.color = Color.red;
            health -= damage;
            _isInvincible = true;
            _invincibilityCount = 0f;
            if (gameObject.CompareTag("Player"))
            {
                PlayerIsDamagedEventHandler?.Invoke(this, new PlayerIsDamagedEventArgs(enemyIndex, damage, health, impactDirection));
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                ((IQuestElement) this._enemyController).OnQuestTaskResolved( this, new QuestDamageEnemyEventArgs(_enemyController.EnemyData.weapon, damage, _enemyController.QuestId));
                _enemyController.CheckDeath();
            }
        }
        
        public bool ApplyHeal(int healing)
        {
            if (GetMaxHealth() <= GetHealth()) return false;
            var newHealth = health + healing;
            health = _maxHealth >= newHealth ? newHealth : _maxHealth;
            return true;
        }

        public void SetHealth(int newHealth)
        {
            if (_maxHealth == -1)
            {
                _maxHealth = newHealth;
            }
            health = newHealth;
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetMaxHealth()
        {
            return _maxHealth;
        }

        public bool IsInvincible()
        {
            return _isInvincible;
        }

        public void SetOriginalColor(Color color)
        {
            _originalColor = color;
        }
    }
}
