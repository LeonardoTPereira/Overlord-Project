using Game.LevelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class DungeonFileSO : ScriptableObject
{
    public Dimensions dimensions;
    public List<SORoom> rooms;

    private int currentIndex = 0;

    public void ResetIndex()
    {
        currentIndex = 0;
    }

    public DungeonPart GetNextPart()
    {
        if (currentIndex < rooms.Count)
            return DungeonPartFactory.CreateDungeonPartFromDungeonFileSO(rooms[currentIndex++]);
        else
            return null;
    }
}
