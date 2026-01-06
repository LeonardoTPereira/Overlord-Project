using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 1f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}