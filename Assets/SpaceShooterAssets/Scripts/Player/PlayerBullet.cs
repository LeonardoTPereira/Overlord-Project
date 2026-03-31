using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 1f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        CheckIfIsInPlayScreen();
    }

    const float MIN_X = -1.5f;
    const float MAX_X = 0.5f;
    const float MIN_Y = -1.0f;
    const float MAX_Y = 1.0f;

    protected void CheckIfIsInPlayScreen()
    {
        Vector3 pos = this.transform.position;

        if (!(pos.x >= MIN_X && pos.x <= MAX_X &&
               pos.y >= MIN_Y && pos.y <= MAX_Y))
            Destroy(gameObject);
    }
}