using System;
using Game.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager.Player
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        protected int maxHealth;
        [SerializeField]
        protected ParticleSystem bloodParticle;
        private Collider2D playerCollider;
        private SpriteRenderer spriteRenderer;
        private HealthController healthController;

        public static event EventHandler PlayerDeathEventHandler;
        public static event EventHandler ResetHealthEventHandler;
        public static event EventHandler SceneLoaded;

        public void Awake()
        {
            healthController = gameObject.GetComponent<HealthController>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            playerCollider = gameObject.GetComponent<Collider2D>();
        }

        // Use this for initialization
        private void Start()
        {
            healthController.SetHealth(maxHealth);
            var originalColor = spriteRenderer.color;
            healthController.SetOriginalColor(originalColor);
        }

        private void OnEnable()
        {
            HealthController.PlayerIsDamagedEventHandler += CheckDeath;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            HealthController.PlayerIsDamagedEventHandler -= CheckDeath;
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "Overworld" || scene.name == "LevelWithEnemies")
            {
                playerCollider.enabled = true;
            }
        }

        private void CheckDeath(object sender, PlayerIsDamagedEventArgs eventArgs)
        {
            var mainParticle= bloodParticle.main;
            mainParticle.startSpeed = 0;
            var forceOverLifetime = bloodParticle.forceOverLifetime;
            forceOverLifetime.enabled = true;
            forceOverLifetime.x = eventArgs.ImpactDirection.x * 20;
            forceOverLifetime.y = eventArgs.ImpactDirection.y * 20;
            forceOverLifetime.z = eventArgs.ImpactDirection.z * 20;

            bloodParticle.Play();
            if (eventArgs.PlayerHealth > 0) return;
            SceneLoaded?.Invoke(null, EventArgs.Empty);
            playerCollider.enabled = false;
            PlayerDeathEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void ResetHealth()
        {
            healthController.SetHealth(maxHealth);
            ResetHealthEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public int GetHealth()
        {
            return healthController.GetHealth();
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public bool IsInvincible()
        {
            return healthController.IsInvincible();
        }
    }
}
