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

    /// The healer healing cooldown.
    private float cooldown;
    /// The elapsed time.
    private float time;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Get the enemy component of this healer
        GameObject healer = gameObject
            .transform.parent.gameObject  // WeaponPosition
            .transform.parent.gameObject  // EnemyArms
            .transform.parent.gameObject; // Enemy
        EnemyController ec = healer.GetComponent<EnemyController>();
        // Calculate the healer healing cooldown
        cooldown = BASE_COOLDOWN * (1.0f / ec.GetAttackSpeed());
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
            other.GetComponent<EnemyController>().Heal(HEALTH);
        }
    }
}
