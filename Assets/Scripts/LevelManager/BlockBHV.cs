using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBHV : TileBHV {


	// Use this for initialization
	void Start () {
			GetComponent<Collider2D> ().enabled = true; // ativa o colisor
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CollidedWithSomething");
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("CollidedWithBullet");
            collision.gameObject.GetComponent<ProjectileController>().DestroyBullet();
        }
    }

}
