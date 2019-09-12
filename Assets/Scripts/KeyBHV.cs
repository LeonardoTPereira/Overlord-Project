using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBHV : PlaceableRoomObject {

	public int keyID;

	// Use this for initialization
	void Start () {
        //Render the key sprite with the color relative to its ID
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Util.colorId[keyID - 1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
            Player.instance.GetKey (keyID);
            GameManager.instance.UpdateKeyGUI();
            //TODO: Analytics to flag when the player got a key
            OnGetKey();
			Destroy (gameObject);
		}
	}

    private void OnGetKey ()
    {
        PlayerProfile.instance.OnGetKey(keyID);
    }
}
