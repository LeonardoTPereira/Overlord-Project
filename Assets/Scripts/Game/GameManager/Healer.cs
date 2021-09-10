using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This class is responsible by healing the enemies that are not healers.
public class Healer : MonoBehaviour
{
    /// Number of seconds by minute.
    private static readonly int SECONDS = 60;
    /// Cooldown duration error.
    private static readonly float COOLDOWN_DURATION_ERROR = 0.005f;
    /// Base healing cooldown.
    private static readonly float BASE_COOLDOWN = 3f;
    /// Amount of health that the healer can recover.
    private static readonly int HEALTH = 1;
    /// Healer cure spell size.
    private static readonly float CURE_SPELL_SIZE = 0.75f;

    /// The healer healing cooldown.
    private float cooldown;
    /// The elapsed time.
    private float time;
    /// Game object of the healer.
    private GameObject healer;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Get the weapon component of the enemy
        GameObject weapon = gameObject
            .transform.parent.gameObject; // WeaponPosition
        // Get the enemy component of this healer
        healer = weapon
            .transform.parent.gameObject  // EnemyArms
            .transform.parent.gameObject; // Enemy
        // Define the position of the healer's arms
        Vector3 armsHeight = new Vector3(0f, 0.2f, 0f);
        Vector3 leftArmPosition = new Vector3(1f, 0f, 0f);
        // Place the spots of cure spell animation
        ParticleSystem cure1 = healer.GetComponentInChildren<ParticleSystem>();
        cure1.transform.localScale *= CURE_SPELL_SIZE;
        cure1.transform.position = weapon.transform.position + armsHeight;
        ParticleSystem cure2 = Instantiate(cure1);
        cure2.transform.parent = healer.transform;
        cure2.transform.position = cure1.transform.position + leftArmPosition;
        // Calculate the healer healing cooldown
        EnemyController ec = healer.GetComponent<EnemyController>();
        cooldown = BASE_COOLDOWN * (1f / ec.GetAttackSpeed());
        time = cooldown;
    }

    /// Update is called once per frame.
    void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
    }

    /// Cure all the enemies around the healer and reset the cooldown.
    void OnTriggerStay2D(Collider2D other)
    {
        float seconds = Mathf.FloorToInt(time % SECONDS);
        if (other.tag == "Enemy" && seconds <= COOLDOWN_DURATION_ERROR)
        {
            time = cooldown;
            if (other.GetComponent<EnemyController>().Heal(HEALTH))
            {
                // Play the cure spell animation in the healer's hands
                foreach (ParticleSystem spell in healer.
                    GetComponentsInChildren<ParticleSystem>())
                {
                    spell.Play();
                }
                // Play the cure spell animation over the cured enemy
                other.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}
