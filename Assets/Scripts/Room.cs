using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public int x, y;
	public static int sizeX, sizeY; // inicializar valores antes de acessar os tiles
	public static bool tiled = false;
	public int[,] tiles = null;
	public int lockID;
	public int keyID;

	public Room(int x, int y){
		this.x = x;
		this.y = y;
		lockID = 0;
		keyID = 0;
	}

	public bool IsRoom(){
		return ( (x % 2) + (y % 2) ) == 0;
	}

	public void InitializeTiles(){ // prepara a memória para receber os valores dos tiles
		tiles = new int[Room.sizeX, Room.sizeY];
	}

}
