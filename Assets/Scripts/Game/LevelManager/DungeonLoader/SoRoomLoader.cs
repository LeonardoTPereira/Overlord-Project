using System;
using System.Collections;
using System.Collections.Generic;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using UnityEngine;
using Util;

public static class SoRoomLoader
{
    public static void CreateRoom(DungeonRoom room, RoomGeneratorInput roomGeneratorInput)
    {
        RoomData newRoom = RandomRoomGenerator.CreateNewRoom(roomGeneratorInput); //Sua geração de salas
        for (var x = 0; x < room.Dimensions.Width; x++)
        {
            for (var y = 0; y < room.Dimensions.Height; y++)
            {
                Debug.Log("My room dimensions: "+newRoom.Room.Length + " " + newRoom.Room[x].Tiles.Length);
                Debug.Log("Room dimensions: "+roomGeneratorInput.Size.x+" "+roomGeneratorInput.Size.y);
                room.Tiles[x, y] = (int)newRoom.Room[x][y].TileType;
            }
        }
    }
}

