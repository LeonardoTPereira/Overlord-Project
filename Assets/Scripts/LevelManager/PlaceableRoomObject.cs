using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableRoomObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void SetRoom(Coordinates coordinates, Dimensions dimensions){
		GameManager gm = GameManager.instance;
		Transform roomTransf = gm.roomBHVMap [x, y].transform;
		Vector2 roomSelfCenter = new Vector2 (DungeonPart.sizeX/2.0f - 0.5f, DungeonPart.sizeY/2.0f - 0.5f);

		DungeonPart room = gm.GetMap ().dungeonPartByCoordinates [x, y];
		float minSqDist = Mathf.Infinity;
		int minX = 0; //será modificado
		int minY = 0; //será modificado
		//Debug.Log("Center: " + roomSelfCenter.x + "," + roomSelfCenter.y);
		for (int ix = 0; ix < DungeonPart.sizeX; ix++){
			for (int iy = 0; iy < DungeonPart.sizeY; iy++){
				//Debug.Log ("Min Dist: " + minSqDist + "; MinX: " + minX + "; MinY: " + minY);
				if (room.tiles[ix, iy] != 1){ //é passável?
					float sqDist = Mathf.Pow(ix - roomSelfCenter.x, 2) + Mathf.Pow(iy - roomSelfCenter.y, 2);
					if (sqDist <= minSqDist){
						minSqDist = sqDist;
						minX = ix;
						minY = iy;
						//Debug.Log ("NEW! Min Dist: " + minSqDist + "; MinX: " + minX + "; MinY: " + minY);
					}
				}
			}
		}
		//Debug.Log ("Min Dist: " + minSqDist + "; MinX: " + minX + "; MinY: " + minY);
		transform.position = new Vector2 (minX, DungeonPart.sizeY -1 - minY) - roomSelfCenter + (Vector2)roomTransf.position;
        //Debug.Log("New room position!");
	}
}
