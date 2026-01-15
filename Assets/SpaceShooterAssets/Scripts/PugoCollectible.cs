using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugoCollectible : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 1;
    [SerializeField] private int _collectibleAmount = 1;

    private void Update()
    {
        Tick();
        if (!IsVisible())
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(_collectibleAmount);
            FindObjectOfType<RoomController>()?.OnCollectedTreasure();
            Destroy(gameObject);
        }
    }
    public void Tick()
    {
        transform.position += Vector3.down * _fallSpeed * Time.deltaTime;

        if (!IsVisible())
            Destroy(gameObject);
    }

    protected bool IsVisible()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > -0.1f && v.x < 1.1f && v.y > -0.1f && v.y < 1.1f;
    }
}
