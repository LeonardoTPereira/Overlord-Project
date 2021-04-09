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
            //GameManager.state = GameManager.LevelPlayState.Won; comentado por Luana e Paolo
            //TODO change this to when the sierpinsk-force is taken
            //OnMapComplete();
            //gm.LevelComplete(); comentado por Luana e Paolo
        }
	}

    private void OnMapComplete (Map currentMap)
    {
        PlayerProfile.instance.OnMapComplete(currentMap);
    }
}
