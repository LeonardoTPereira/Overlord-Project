using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float range;
    private Vector3 startPos;

    public void Init(Vector2 dir, float spd, float rng)
    {
        direction = dir.normalized;
        speed = spd;
        range = rng;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) > range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponentInParent<PlayerShip>()?.TakeDamage();
            Destroy(gameObject);
        }
    }
}
