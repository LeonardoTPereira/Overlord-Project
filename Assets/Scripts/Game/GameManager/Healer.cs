using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    private static readonly float ERROR = 0.005f;
    private static readonly float COOLDOWN = 3f;
    private static readonly int SECONDS = 60;
    private float cooldown;
    private float time;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        GameObject healer = gameObject
            .transform.parent.gameObject  // WeaponPosition
            .transform.parent.gameObject  // EnemyArms
            .transform.parent.gameObject; // Enemy
        EnemyController ec = healer.GetComponent<EnemyController>();
        cooldown = COOLDOWN * (1.0f / ec.GetAttackSpeed());
        time = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
    }

    // Cure all the enemies around the healer and reset the cooldown
    void OnTriggerStay2D(Collider2D other)
    {
        float seconds = Mathf.FloorToInt(time % SECONDS);
        if (other.tag == "Enemy" && seconds <= ERROR)
        {
            time = cooldown;
            other.GetComponent<EnemyController>().Cure(1);
        }
    }
}
