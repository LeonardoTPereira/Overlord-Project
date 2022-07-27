using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameManager
{
    /// This class is responsible by healing the enemies that are not healers.
    public class Healer : MonoBehaviour
    {
        /// Base healing cooldown.
        private static readonly float BASE_COOLDOWN = 4f;
        /// Base healing cooldown.
        private static readonly float HEAL_AREA_COOLDOWN = 0.75f;
        /// Alpha channel value when heal area is active.
        private static readonly float HEAL_AREA_ALPHA_ACTIVE = 0.177f;
        /// Alpha channel value when heal area is deactive.
        private static readonly float HEAL_AREA_ALPHA_DEACTIVE = 0f;
        /// Amount of health that the healer can recover.
        private static readonly int HEALTH = 1;
        /// Healer cure spell size.
        private static readonly float CURE_SPELL_SIZE = 0.75f;

        /// The healer healing cooldown.
        private float cooldown;
        /// Game object of the healer.
        private GameObject healer;
        /// Heal area.
        private SpriteRenderer healArea;
        private bool healed;
        private List<Collider2D> enemies;

        /// Awake is called when the script instance is being loaded.
        void Awake()
        {
            healed = false;
            enemies = new List<Collider2D>();
            // Get the weapon component of the enemy
            GameObject weapon = gameObject
                .transform.parent.gameObject; // WeaponPosition
            // Get the enemy component of this healer
            healer = weapon
                .transform.parent.gameObject  // EnemyArms
                .transform.parent.gameObject; // Enemy
            // Define the position of the healer's arms
            Vector3 armsHeight = new Vector3(0f, 0.2f, 0f);
            // Place the spots of cure spell animation
            ParticleSystem spell = healer.GetComponentInChildren<ParticleSystem>();
            spell.transform.localScale *= CURE_SPELL_SIZE;
            spell.transform.position = weapon.transform.position + armsHeight;
            // Calculate the healer healing cooldown
            EnemyController ec = healer.GetComponent<EnemyController>();
            cooldown = BASE_COOLDOWN * (1f / ec.GetAttackSpeed());
            // Hide heal area
            healArea = gameObject.GetComponent<SpriteRenderer>();
            HideHealArea();
        }

        void FixedUpdate()
        {
            if (!healed)
            {
                StartCoroutine(Heal());
            }
        }

        /// Update the alpha channel of the heal area sprite.
        private void UpdateHealAreaAlpha(float alpha)
        {
            float r = healArea.color.r;
            float g = healArea.color.g;
            float b = healArea.color.b;
            healArea.color = new Color(r, g, r, alpha);
        }

        /// Hide the heal area.
        private void HideHealArea()
        {
            UpdateHealAreaAlpha(HEAL_AREA_ALPHA_DEACTIVE);
        }

        /// Show the heal area.
        private void ShowHealArea()
        {
            UpdateHealAreaAlpha(HEAL_AREA_ALPHA_ACTIVE);
        }

        /// Heal other non-healer enemies.
        IEnumerator Heal()
        {
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.GetComponent<EnemyController>().Heal(HEALTH))
                {
                    healed = true;
                    // Play the cure spell animation in the healer's hands
                    foreach (ParticleSystem spell in healer.
                                 GetComponentsInChildren<ParticleSystem>())
                    {
                        spell.Play();
                    }
                    // Play the cure spell animation over the cured enemy
                    enemy.GetComponentInChildren<ParticleSystem>().Play();
                }
            }
            if (healed)
            {
                ShowHealArea();
                yield return new WaitForSeconds(HEAL_AREA_COOLDOWN);
                HideHealArea();
                yield return new WaitForSeconds(cooldown);
                healed = false;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                enemies.Add(other);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                enemies.Remove(other);
            }
        }
    }
}
