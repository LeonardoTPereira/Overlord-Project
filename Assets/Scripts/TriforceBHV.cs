using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriforceBHV : PlaceableRoomObject {

    GameManager gm;
    private void Start()
    {
        gm = GameManager.instance;
    }
    void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
            GameManager.state = GameManager.LevelPlayState.Won;
            //TODO change this to when the sierpinsk-force is taken
            //OnMapComplete();
            gm.LevelComplete();
        }
	}

    private void OnMapComplete ()
    {
        PlayerProfile.instance.OnMapComplete();
    }
}
